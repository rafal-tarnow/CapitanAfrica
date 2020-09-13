#pragma once
#include <QObject>
#include <QTcpSocket>
#include <QDataStream>


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
    void startTransfer(QString textToTransfer);
    void disconnect();
    void abort();

private slots:
    void readyReadFromSocket();
    void displayError(QAbstractSocket::SocketError socketError);

private:
    QTcpSocket *clientSocket;
    QDataStream in;
};
