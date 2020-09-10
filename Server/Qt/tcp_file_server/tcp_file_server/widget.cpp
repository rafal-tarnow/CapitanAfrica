#include "widget.h"
#include "ui_widget.h"
#include <QMessageBox>
#include <QNetworkInterface>
#include <QRandomGenerator>
#include <QTcpSocket>

Widget::Widget(QWidget *parent)
    : QWidget(parent)
    , ui(new Ui::Widget)
{
    ui->setupUi(this);

    tcpServer = new QTcpServer(this);
    if (!tcpServer->listen(QHostAddress::Any,1234)) {
        QMessageBox::critical(this, tr("Fortune Server"),
                              tr("Unable to start the server: %1.")
                              .arg(tcpServer->errorString()));
        close();
        return;
    }

    showIpAndPortOnLabel();

    connect(tcpServer, &QTcpServer::newConnection, this, &Widget::sendFortune);
}

Widget::~Widget()
{
    delete ui;
    delete tcpServer;
}

void Widget::sendFortune()
{
//! [5]
    QByteArray block;
    QDataStream out(&block, QIODevice::WriteOnly);
    out.setVersion(QDataStream::Qt_5_10);

    QString text1 = "Text1";
    QString text2 = "Text2";

    static bool text_flag = false;

    if(text_flag)
        out << text1;
    else
        out << text2;
    text_flag = !text_flag;


    QTcpSocket *clientConnection = tcpServer->nextPendingConnection();
    connect(clientConnection, &QAbstractSocket::disconnected,
            clientConnection, &QObject::deleteLater);
//! [7] //! [8]

    clientConnection->write(block);
    clientConnection->disconnectFromHost();
//! [5]
}

void Widget::showIpAndPortOnLabel()
{
    QString ipAddress;
    QList<QHostAddress> ipAddressesList = QNetworkInterface::allAddresses();
    // use the first non-localhost IPv4 address
    for (int i = 0; i < ipAddressesList.size(); ++i) {
        if (ipAddressesList.at(i) != QHostAddress::LocalHost &&
            ipAddressesList.at(i).toIPv4Address()) {
            ipAddress = ipAddressesList.at(i).toString();
            break;
        }
    }
    // if we did not find one, use IPv4 localhost
    if (ipAddress.isEmpty())
        ipAddress = QHostAddress(QHostAddress::LocalHost).toString();
    ui->statusLabel->setText(tr("The server is running on\n\nIP: %1\nport: %2\n\n"
                            "Run the Fortune Client example now.")
                         .arg(ipAddress).arg(tcpServer->serverPort()));
}
