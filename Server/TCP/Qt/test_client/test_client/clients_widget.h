#ifndef WIDGET_H
#define WIDGET_H

#include <QWidget>
#include <QTcpSocket>
#include <QHostAddress>
#include "../../libs/client.hpp"

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

private slots:
    void on_pushButtonSelectFile_clicked();
    void on_pushButtonNewClient_clicked();

private:
    QHostAddress getIP();
    void startTransfer(QString textToTransfer);

private:
    Ui::Widget *ui;
    int connectionCount = 0;
};
#endif // WIDGET_H
