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

    ui->pushButtonGetFortune->setDefault(true);
    //ui->pushButtonGetFortune->setEnabled(false);

    tcpSocket = new QTcpSocket(this);

        in.setDevice(tcpSocket);
        in.setVersion(QDataStream::Qt_4_0);

        connect(tcpSocket, &QIODevice::readyRead, this, &Widget::readFortune);
    //! [2] //! [4]
        connect(tcpSocket, QOverload<QAbstractSocket::SocketError>::of(&QAbstractSocket::error),
    //! [3]
                this, &Widget::displayError);
}

Widget::~Widget()
{
    delete ui;
    delete tcpSocket;
}

void Widget::requestNewFortune()
{
    ui->pushButtonGetFortune->setEnabled(false);
    tcpSocket->abort();
//! [7]
    tcpSocket->connectToHost(getIP().toString(),1234);
//! [7]
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

void Widget::on_pushButtonGetFortune_clicked()
{
    requestNewFortune();
}

void Widget::readFortune()
{
    in.startTransaction();

    QString nextFortune;
    in >> nextFortune;

    if (!in.commitTransaction())
        return;

    if (nextFortune == currentFortune) {
        QTimer::singleShot(0, this, &Widget::requestNewFortune);
        return;
    }

    currentFortune = nextFortune;
    ui->statusLabel->setText(currentFortune);
    ui->pushButtonGetFortune->setEnabled(true);
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

    ui->pushButtonGetFortune->setEnabled(true);
}


