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
    void readFortune();
    void displayError(QAbstractSocket::SocketError socketError);
    void requestNewFortune();

private slots:
    void on_pushButtonSelectFile_clicked();

    void on_pushButtonGetFortune_clicked();

private:
    QHostAddress getIP();

private:
    Ui::Widget *ui;
    QTcpSocket *tcpSocket = nullptr;
    QDataStream in;

     QString currentFortune;
};
#endif // WIDGET_H
