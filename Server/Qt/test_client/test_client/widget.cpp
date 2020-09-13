#include "widget.h"
#include "ui_widget.h"
#include <QFileDialog>
#include <QStandardPaths>
#include <QNetworkInterface>
#include <QMessageBox>
#include <QTimer>

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
}

void Widget::on_pushButtonConnect_clicked()
{
    tcpSocket->connectToHost(getIP().toString(),1234);
}

void Widget::on_pushButtonStartTransfer_clicked()
{
    startTransfer("Data form client!");
}

void Widget::on_pushButtonAbord_clicked()
{
    tcpSocket->abort();
}

void Widget::startTransfer(QString textToTransfer)
{
    qDebug() << "startTransfer()";

    QByteArray block;
    QDataStream out(&block, QIODevice::WriteOnly);
    out.setVersion(QDataStream::Qt_5_10);

    out << textToTransfer;

    tcpSocket->write(block);
}

void Widget::readyReadFromSocket()
{
    in.startTransaction();

    QString nextFortune;
    in >> nextFortune;

    if (!in.commitTransaction())
        return;

    ui->statusLabel->setText(nextFortune);
}

void Widget::on_pushButtonSelectFile_clicked()
{
    QString fileName = QFileDialog::getOpenFileName(this,
                                                    tr("Open Image"), QStandardPaths::writableLocation(QStandardPaths::DesktopLocation), tr("Image Files (*.png *.jpg *.bmp)"));
}


void Widget::displayError(QAbstractSocket::SocketError socketError)
{
    switch (socketError) {
    case QAbstractSocket::RemoteHostClosedError:
        break;
    case QAbstractSocket::HostNotFoundError:
        QMessageBox::information(this, tr("Fortune Client"),
                                 tr("The host was not found. Please check the "
                                    "host name and port settings."));
        break;
    case QAbstractSocket::ConnectionRefusedError:
        QMessageBox::information(this, tr("Fortune Client"),
                                 tr("The connection was refused by the peer. "
                                    "Make sure the fortune server is running, "
                                    "and check that the host name and port "
                                    "settings are correct."));
        break;
    default:
        QMessageBox::information(this, tr("Fortune Client"),
                                 tr("The following error occurred: %1.")
                                 .arg(tcpSocket->errorString()));
    }
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








