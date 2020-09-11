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
    getFortuneButton->setEnabled(false);
    tcpSocket->abort();
//! [7]
    tcpSocket->connectToHost(hostCombo->currentText(),
                             portLineEdit->text().toInt());
//! [7]
}

void Widget::readFortune()
{
    in.startTransaction();

    QString nextFortune;
    in >> nextFortune;

    if (!in.commitTransaction())
        return;

    if (nextFortune == currentFortune) {
        QTimer::singleShot(0, this, &Client::requestNewFortune);
        return;
    }

    currentFortune = nextFortune;
    statusLabel->setText(currentFortune);
    getFortuneButton->setEnabled(true);
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

    getFortuneButton->setEnabled(true);
}
