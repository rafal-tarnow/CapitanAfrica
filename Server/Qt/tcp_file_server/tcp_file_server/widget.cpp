#include "widget.h"
#include "ui_widget.h"
#include <QMessageBox>
#include <QNetworkInterface>
#include <QRandomGenerator>
#include <QTcpSocket>


Client::Client()
{
    qDebug() << "New Cient";
}

Client::~Client()
{
    qDebug() << "Delete Client";
}

Widget::Widget(QWidget *parent)
    : QWidget(parent)
    , ui(new Ui::Widget)
{
    ui->setupUi(this);

    tcpServer = new QTcpServer(this);

}

Widget::~Widget()
{
    delete ui;
    delete tcpServer;
}


void Widget::on_pushButton_startListenForConnections_clicked()
{
    if (!tcpServer->listen(QHostAddress::Any,1234))
    {
        QMessageBox::critical(this, tr("Fortune Server"),tr("Unable to start the server: %1.").arg(tcpServer->errorString()));
        close();
        return;
    }
    connect(tcpServer, &QTcpServer::newConnection, this, &Widget::acceptConnection);


    showIpAndPortOnLabel();
}

void Widget::acceptConnection()
{ 
    QTcpSocket *clientSocket = tcpServer->nextPendingConnection();
    Client * client = new Client();
    //connect(clientConnection,SIGNAL(connected()), this, SLOT(startTransfer()));
    //connect(clientConnection, SIGNAL(bytesWritten(qint64)), this, SLOT(updateServerProgress(qint64)));
    //connect(clientConnection, SIGNAL(error(QAbstractSocket::SocketError)), this, SLOT(displayError(QAbstractSocket::SocketError)));
    connect(clientSocket, &QAbstractSocket::disconnected,clientSocket, &QObject::deleteLater);
    connect(clientSocket, &QAbstractSocket::disconnected,client, &QObject::deleteLater);



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



    //! [7] //! [8]

    clientSocket->write(block);
    clientSocket->disconnectFromHost();
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


