#include "widget.h"
#include "ui_widget.h"
#include <QMessageBox>
#include <QNetworkInterface>
#include <QRandomGenerator>
#include <QTcpSocket>
#include <QMessageBox>


ServerClient::ServerClient(QTcpSocket *socket)
{
    qDebug() << "New Cient";
    clientSocket = socket;

    connect(clientSocket,SIGNAL(connected()), this, SLOT(startTransfer()));
    //connect(clientConnection, SIGNAL(bytesWritten(qint64)), this, SLOT(updateServerProgress(qint64)));
    connect(clientSocket, SIGNAL(error(QAbstractSocket::SocketError)), this, SLOT(displayError(QAbstractSocket::SocketError)));
    connect(clientSocket, &QAbstractSocket::disconnected,this, &QObject::deleteLater);
    connect(clientSocket, &QAbstractSocket::disconnected,clientSocket, &QObject::deleteLater);


}
void ServerClient::startTransfer()
{
    qDebug() << "startTransfer()";
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

ServerClient::~ServerClient()
{
    qDebug() << "Delete Client";
}

void ServerClient::displayError(QAbstractSocket::SocketError socketError)
{
    switch (socketError) {
    case QAbstractSocket::RemoteHostClosedError:
        break;
    case QAbstractSocket::HostNotFoundError:
        QMessageBox::information(nullptr, tr("Fortune Client"),
                                 tr("The host was not found. Please check the "
                                    "host name and port settings."));
        break;
    case QAbstractSocket::ConnectionRefusedError:
        QMessageBox::information(nullptr, tr("Fortune Client"),
                                 tr("The connection was refused by the peer. "
                                    "Make sure the fortune server is running, "
                                    "and check that the host name and port "
                                    "settings are correct."));
        break;
    default:
        QMessageBox::information(nullptr, tr("Fortune Client"),
                                 tr("The following error occurred: %1.")
                                 .arg(clientSocket->errorString()));
    }
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
    ServerClient * client = new ServerClient(clientSocket);
    client->startTransfer();
}


void Widget::showIpAndPortOnLabel()
{
    QString ipAddress;
    ipAddress = getIP().toString();

    ui->statusLabel->setText(tr("The server is running on\n\nIP: %1\nport: %2\n\n"
                                "Run the Fortune Client example now.")
                             .arg(ipAddress).arg(tcpServer->serverPort()));
}

QHostAddress Widget::getIP()
{
    QHostAddress address;
    QList<QHostAddress> ipAddressesList = QNetworkInterface::allAddresses();
    // use the first non-localhost IPv4 address
    for (int i = 0; i < ipAddressesList.size(); ++i) {
        if (ipAddressesList.at(i) != QHostAddress::LocalHost &&
                ipAddressesList.at(i).toIPv4Address()) {
            return ipAddressesList.at(i);
        }
    }
    // if we did not find one, use IPv4 localhost
    return QHostAddress(QHostAddress::LocalHost);
}


