#include "client.hpp"
#include <QDataStream>
#include <QMessageBox>
#include <QFileInfo>
#include <QStandardPaths>

Client::Client(QTcpSocket *socket)
{
    qDebug() << "New Cient";
    clientSocket = socket;

    in.setDevice(clientSocket);
    in.setVersion(QDataStream::Qt_4_0);

    connect(clientSocket, SIGNAL(bytesWritten(qint64)), this, SLOT(bytesWrittenFromSocket(qint64)));
    connect(clientSocket, &QIODevice::readyRead, this, &Client::readyReadFromSocket);
    connect(clientSocket, SIGNAL(error(QAbstractSocket::SocketError)), this, SLOT(displayError(QAbstractSocket::SocketError)));
    connect(clientSocket, &QAbstractSocket::disconnected,this, &QObject::deleteLater);
    connect(clientSocket, &QAbstractSocket::disconnected,clientSocket, &QObject::deleteLater);

}

void Client::connectTo(const QHostAddress &address, quint16 port)
{
    clientSocket->connectToHost(address,port);
}

//void Client::startTransferFile(QString filePath)
//{
//    fileToSend = new QFile(filePath);
//    if (!fileToSend->open(QIODevice::ReadOnly))
//    {
//        qDebug() << "Couldn't open the file";
//        return;
//    }
//    int TotalBytes = fileToSend->size();

//    bytesToWrite = TotalBytes - (int)clientSocket->write(fileToSend->read(PayloadSize));
//}

void Client::startTransferFile(QString filePath)
{
    QDataStream out(clientSocket);
    out.setVersion(QDataStream::Qt_5_11);

    QFile file(filePath);
    if(file.open(QIODevice::ReadOnly)){
        qDebug("File opened");
    }else{
        return;
    }
    QFileInfo info(filePath);
    out << info.fileName() << (int)file.size() << file.read(file.size());




//    fileToSend = new QFile(filePath);
//    if(fileToSend->open(QIODevice::ReadOnly)){
//        qDebug("File opened");
//    }

//    QByteArray content;
//    QDataStream out(&content, QIODevice::WriteOnly);
//    out.setVersion(QDataStream::Qt_4_7);

//    out << static_cast<quint16>(0) << filePath << fileToSend->read(fileToSend->size());


//    out.device()->seek(0);
//    out << static_cast<quint16>(content.size());

//    qDebug() << "content.size() = " << content.size();

//    qDebug() << "clientSocket->write(content) = " << clientSocket->write(content);

    //--------------
//    qDebug() << "startTransfer()";

//    QFile file(filePath);
//    file.open(QIODevice::ReadOnly);

//    QDataStream inFile(&file);

//    QDataStream out(clientSocket);
//    out.setVersion(QDataStream::Qt_5_10);

//    out << inFile;

//    file.close();

//    //clientSocket->write(block);
}

void Client::bytesWrittenFromSocket(qint64 numBytes)
{
    bytesWritten += (int)numBytes;
    qDebug() << "bytesWritten = " << bytesWritten;
//    qDebug() << "bytesToWrite = " << bytesToWrite;

//    // only write more if not finished and when the Qt write buffer is below a certain size.
//    if (bytesToWrite > 0)
//    {
//        bytesToWrite -= (int)clientSocket->write(fileToSend->read(PayloadSize - clientSocket->bytesToWrite()));
//    }
//    else if(bytesToWrite == 0)
//    {
//        fileToSend->close();
//        delete fileToSend;
//    }

}

void Client::startTransferMessage(QString textToTransfer)
{
    qDebug() << "startTransfer()";

    QDataStream out(clientSocket);
    out.setVersion(QDataStream::Qt_5_10);

    out << textToTransfer;

    //clientSocket->write(block);
}

void Client::disconnect()
{
    clientSocket->disconnectFromHost();
}
void Client::abort()
{
    clientSocket->abort();
}

//void Client::readyReadFromSocket()
//{
//    in.startTransaction();

//    QString nextFortune;
//    in >> nextFortune;

//    if (!in.commitTransaction())
//        return;

//    emit textArrive(nextFortune);
//}

void Client::readyReadFromSocket()
{
    in.startTransaction();

    QString fileName;
    int fileSize;

    in >> fileName;
    in >> fileSize;

    QFile file(QStandardPaths::writableLocation(QStandardPaths::DesktopLocation) + "/SERVER/" + fileName);
    file.open(QIODevice::WriteOnly);
    QDataStream out(&file);
    QByteArray buff;
    in >> buff;
    out << buff;
    file.close();


    if (!in.commitTransaction())
        return;



    emit textArrive(fileName + QString::number(fileSize));


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
