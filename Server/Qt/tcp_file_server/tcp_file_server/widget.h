#ifndef WIDGET_H
#define WIDGET_H

#include <QWidget>
#include <QTcpServer>

QT_BEGIN_NAMESPACE
namespace Ui { class Widget; }
QT_END_NAMESPACE

class Widget : public QWidget
{
    Q_OBJECT

public:
    Widget(QWidget *parent = nullptr);
    ~Widget();

private slots:
    void on_pushButton_startListenForConnections_clicked();

private:
    QHostAddress getIP();
    void acceptConnection();
    void showIpAndPortOnLabel();

    Ui::Widget *ui;
    QTcpServer * tcpServer;
};


class ServerClient : public QObject
{
    Q_OBJECT

public:
    ServerClient(QTcpSocket *clientSocket);
    ~ServerClient();

public slots:
    void startTransfer();

private slots:
    void displayError(QAbstractSocket::SocketError socketError);




private:
    QTcpSocket *clientSocket;
};

#endif // WIDGET_H
