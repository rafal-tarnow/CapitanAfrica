#include "widget.h"
#include "ui_widget.h"
#include <QFileDialog>
#include <QStandardPaths>

Widget::Widget(QWidget *parent)
    : QWidget(parent)
    , ui(new Ui::Widget)
{
    ui->setupUi(this);

    tcpSocket = new QTcpSocket(this);
}

Widget::~Widget()
{
    delete ui;
    delete tcpSocket;
}


void Widget::on_pushButtonSelectFile_clicked()
{
    QString fileName = QFileDialog::getOpenFileName(this,
        tr("Open Image"), QStandardPaths::writableLocation(QStandardPaths::DesktopLocation), tr("Image Files (*.png *.jpg *.bmp)"));
}
