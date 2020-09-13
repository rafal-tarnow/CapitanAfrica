#ifndef WIDGET_H
#define WIDGET_H

#include <QWidget>
#include <QTcpSocket>
#include <QHostAddress>

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
    void readyReadFromSocket();
    void displayError(QAbstractSocket::SocketError socketError);

private slots:
    void on_pushButtonSelectFile_clicked();
    void on_pushButtonConnect_clicked();
    void on_pushButtonAbord_clicked();

    void on_pushButtonStartTransfer_clicked();

private:
    QHostAddress getIP();
    void startTransfer(QString textToTransfer);

private:
    Ui::Widget *ui;
    QTcpSocket *tcpSocket = nullptr;
    QDataStream in;
};
#endif // WIDGET_H
