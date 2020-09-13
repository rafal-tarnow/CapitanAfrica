#include "clients_widget.h"
#include "ui_clients_widget.h"
#include <QFileDialog>
#include <QStandardPaths>
#include <QNetworkInterface>
#include <QMessageBox>
#include <QTimer>
#include <QLabel>
#include <QLineEdit>

Widget::Widget(QWidget *parent)
    : QWidget(parent)
    , ui(new Ui::Widget)
{
    ui->setupUi(this);





}

Widget::~Widget()
{
    delete ui;
}

void Widget::on_pushButtonNewClient_clicked()
{
    connectionCount++;

    QTcpSocket *tcpSocket = new QTcpSocket(this);
    Client *client = new Client(tcpSocket);

    QLabel * label = new QLabel("Connection " + QString::number(connectionCount));
    QPushButton * button_connect = new QPushButton("Connect");
    QLineEdit * lineEdit = new QLineEdit();
    QPushButton *button_startTransfer = new QPushButton("Start transfer");
    QPushButton *button_disconnect = new QPushButton("Disconnect");
    QLabel * label_text_from_server = new QLabel("");

    connect(button_connect, &QPushButton::clicked,
            [client, this]() { client->connectTo(this->getIP(),1234);});
    connect(button_startTransfer, &QPushButton::clicked,
            [client, lineEdit]() { client->startTransfer(lineEdit->text()); }
    );
    connect(client, &Client::textArrive, label_text_from_server, &QLabel::setText);

    QGridLayout *layout = ui->gridLayout_2;
    int column  = connectionCount - 1;
    layout->addWidget(label, 0, column);
    layout->addWidget(button_connect, 1, column);
    layout->addWidget(lineEdit, 2, column);
    layout->addWidget(button_startTransfer, 3, column);
    layout->addWidget(button_disconnect, 4, column);
    layout->addWidget(label_text_from_server, 5, column);
}

void Widget::on_pushButtonSelectFile_clicked()
{
    QString fileName = QFileDialog::getOpenFileName(this,
                                                    tr("Open Image"), QStandardPaths::writableLocation(QStandardPaths::DesktopLocation), tr("Image Files (*.png *.jpg *.bmp)"));
}

QHostAddress Widget::getIP()
{
    QHostAddress address;
    QList<QHostAddress> ipAddressesList = QNetworkInterface::allAddresses();
    // use the first non-localhost IPv4 address
    for (int i = 0; i < ipAddressesList.size(); ++i) {
        if (ipAddressesList.at(i) != QHostAddress::LocalHost &&
                ipAddressesList.at(i).toIPv4Address()) {
            return ipAddressesList.at(i);
        }
    }
    // if we did not find one, use IPv4 localhost
    return QHostAddress(QHostAddress::LocalHost);
}


