#ifndef WIDGET_H
#define WIDGET_H

#include <QWidget>
#include <QTcpSocket>

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

private:
    Ui::Widget *ui;
    QTcpSocket *tcpSocket = nullptr;
    QDataStream in;
};
#endif // WIDGET_H
