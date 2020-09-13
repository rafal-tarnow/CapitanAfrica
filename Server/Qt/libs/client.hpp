#pragma once
#include <QObject>
#include <QTcpSocket>
#include <QDataStream>


class ServerClient : public QObject
{
    Q_OBJECT

public:
    ServerClient(QTcpSocket *clientSocket);
    ~ServerClient();

signals:
    void textArrive(QString text);

public slots:
    void connectTo(const QHostAddress &address, quint16 port);
    void startTransfer(QString textToTransfer);
    void disconnect();

private slots:
    void readyReadFromSocket();
    void displayError(QAbstractSocket::SocketError socketError);

private:
    QTcpSocket *clientSocket;
    QDataStream in;
};
