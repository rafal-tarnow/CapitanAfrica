#pragma once
#include <QObject>
#include <QTcpSocket>
#include <QDataStream>
#include <QFile>


class Client : public QObject
{
    Q_OBJECT

public:
    Client(QTcpSocket *clientSocket);
    ~Client();

signals:
    void textArrive(QString text);

public slots:
    void connectTo(const QHostAddress &address, quint16 port);
    void startTransferMessage(QString textToTransfer);
    void startTransferFile(QString filePath);
    void disconnect();
    void abort();

private slots:
    void readyReadFromSocket();
    void displayError(QAbstractSocket::SocketError socketError);
    void bytesWrittenFromSocket(qint64 numBytes);

private:
    QTcpSocket *clientSocket;
    QDataStream in;
    QDataStream out;
    QFile * fileToSend;
    int bytesWritten = 0;
    int bytesToWrite = 0;
    const int PayloadSize = 60000;
};
