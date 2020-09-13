#include "widget.h"
#include "ui_widget.h"
#include <QMessageBox>
#include <QNetworkInterface>
#include <QRandomGenerator>
#include <QTcpSocket>
#include <QMessageBox>
#include <QGridLayout>
#include <QLineEdit>




Widget::Widget(QWidget *parent)
    : QWidget(parent)
    , ui(new Ui::Widget)
{
    ui->setupUi(this);

    tcpServer = new QTcpServer(this);

}

Widget::~Widget()
{
    delete ui;
    delete tcpServer;
}


void Widget::on_pushButton_startListenForConnections_clicked()
{
    if (!tcpServer->listen(QHostAddress::Any,1234))
    {
        QMessageBox::critical(this, tr("Fortune Server"),tr("Unable to start the server: %1.").arg(tcpServer->errorString()));
        close();
        return;
    }
    connect(tcpServer, &QTcpServer::newConnection, this, &Widget::acceptConnection);


    showIpAndPortOnLabel();
}

void Widget::acceptConnection()
{ 
    connectionCount++;

    QTcpSocket *clientSocket = tcpServer->nextPendingConnection();
    ServerClient * client = new ServerClient(clientSocket);


    client->startTransfer("TEKST" + QString::number(connectionCount));


    QLabel * label = new QLabel("Connection " + QString::number(connectionCount));
    QLineEdit * lineEdit = new QLineEdit();
    QPushButton *button1 = new QPushButton("Start transfer");
    QPushButton *button2 = new QPushButton("Disconnect");
    QLabel * label_text_from_client = new QLabel("");

    connect(client, &ServerClient::textArrive, label_text_from_client, &QLabel::setText);
    connect(button1, &QPushButton::clicked,
        [client, lineEdit]() { client->startTransfer(lineEdit->text()); }
    );


    QGridLayout *layout = ui->gridLayout;
    int column  = connectionCount - 1;
    layout->addWidget(label, 0, column);
    layout->addWidget(lineEdit, 1, column);
    layout->addWidget(button1, 2, column);
    layout->addWidget(button2, 3, column);
    layout->addWidget(label_text_from_client, 4, column);
}


void Widget::showIpAndPortOnLabel()
{
    QString ipAddress;
    ipAddress = getIP().toString();

    ui->statusLabel->setText(tr("The server is running on\n\nIP: %1\nport: %2\n\n"
                                "Run the Fortune Client example now.")
                             .arg(ipAddress).arg(tcpServer->serverPort()));
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


