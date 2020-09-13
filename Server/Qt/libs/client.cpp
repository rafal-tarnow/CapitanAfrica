#include "client.hpp"
#include <QDataStream>
#include <QMessageBox>

Client::Client(QTcpSocket *socket)
{
    qDebug() << "New Cient";
    clientSocket = socket;

    in.setDevice(clientSocket);
    in.setVersion(QDataStream::Qt_4_0);

    //connect(clientConnection, SIGNAL(bytesWritten(qint64)), this, SLOT(updateServerProgress(qint64)));
    connect(clientSocket, &QIODevice::readyRead, this, &Client::readyReadFromSocket);
    connect(clientSocket, SIGNAL(error(QAbstractSocket::SocketError)), this, SLOT(displayError(QAbstractSocket::SocketError)));
    connect(clientSocket, &QAbstractSocket::disconnected,this, &QObject::deleteLater);
    connect(clientSocket, &QAbstractSocket::disconnected,clientSocket, &QObject::deleteLater);

}

void Client::connectTo(const QHostAddress &address, quint16 port)
{
    clientSocket->connectToHost(address,port);
}

void Client::startTransfer(QString textToTransfer)
{
    qDebug() << "startTransfer()";

    QByteArray block;
    QDataStream out(&block, QIODevice::WriteOnly);
    out.setVersion(QDataStream::Qt_5_10);

    out << textToTransfer;

    clientSocket->write(block);
}

void Client::disconnect()
{
    clientSocket->disconnectFromHost();
}
void Client::abort()
{
    clientSocket->abort();
}

void Client::readyReadFromSocket()
{
    in.startTransaction();

    QString nextFortune;
    in >> nextFortune;

    if (!in.commitTransaction())
        return;

    emit textArrive(nextFortune);
}

Client::~Client()
{
    qDebug() << "Delete Client";
}

void Client::displayError(QAbstractSocket::SocketError socketError)
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
