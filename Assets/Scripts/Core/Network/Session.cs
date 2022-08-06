using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public enum E_PROTOCOL
{
    CRYPTOKEY,      // 서버 -> 클라				:	초기 암복호화키 전송 신호
    SPAWN,          // 서버 -> 클라, 클라 -> 서버 :  스폰 신호
    MOVE,           // 서버 -> 클라, 클라 -> 서버 :  이동 신호
    EXIT,           // 서버 -> 클라, 클라 -> 서버 :  종료 신호	

    INUSER = 10,
    MOVEUSER = 20,
    OUTUSER = 30,
};
public class Session
{
    SocketEx socket;

    bool m_IsGetKey;

    // uint => unsigned int
    uint m_cryptionMainKey;
    uint m_cryptionSubKey1;
    uint m_cryptionSubKey2;

    public Thread recvThread;
    public Thread sendThread;
    public Session()
    {
        socket = new SocketEx();
    }

    public void TestTreadEnd()
    {
        recvThread.Join();
        sendThread.Join();
        autoSendEndEvent.WaitOne();
        autoRecvEndEvent.WaitOne();
    }
    public void CloseSocket()
    {
        socket.CloseSocket();
    }

    public bool CheckConnecting()
    {
        return socket.m_isConnected;
    }

    public bool Initialize()
    {
        if (socket.Connect("127.0.0.1", 9000))
        {
            sendThread = new Thread(new ThreadStart(SendThread));
            recvThread = new Thread(new ThreadStart(RecvThread));
            sendThread.Start();
            recvThread.Start();
        }
        return true;
    }

    static AutoResetEvent autoEvent = new AutoResetEvent(false);
    static AutoResetEvent autoSendEndEvent = new AutoResetEvent(false);
    static AutoResetEvent autoRecvEndEvent = new AutoResetEvent(false);

    private bool _running = true;
    private Queue<byte[]> _sendQ = new Queue<byte[]>();
    private Queue<byte[]> _recvQ = new Queue<byte[]>();

    void SendThread()
    {
        int l_sendSize = 0;

        while (_running)
        {
            autoEvent.WaitOne();
            if (!_running)
            {
                break;
            }
            while (_sendQ.Count > 0)
            {
                byte[] sendBuffer = _sendQ.Dequeue();
                l_sendSize = BitConverter.ToInt32(sendBuffer, 0) + 4;

                // encryption
                if (m_IsGetKey)
                {
                    Encryption(l_sendSize, sendBuffer);
                }
                socket.Send(sendBuffer, l_sendSize);
            }
        }
        autoSendEndEvent.Set();
    }
    void RecvThread()
    {
        int l_recvedSize = 0;

        while (_running)
        {
            byte[] recvBuffer = new byte[2048];
            if (socket.Receive(recvBuffer, out l_recvedSize))
            {
                // decryption
                if (m_IsGetKey)
                {
                    Decryption(l_recvedSize, recvBuffer);
                }

                if (BitConverter.ToInt32(recvBuffer, 0) == (int)E_PROTOCOL.EXIT)
                {
                    _running = false;
                    autoEvent.Set();
                }
                else
                {
                    if (RecvPacketCountUp(BitConverter.ToInt32(recvBuffer, 4)))
                    {
                        _recvQ.Enqueue(recvBuffer);
                    }
                }
            }
        }
        autoRecvEndEvent.Set();
    }

    const int MAXPACKETNUM = 210000000;
    int m_sendCounter = 0;
    int m_recvCounter = 0;
    private int SendPacketCountUp()
    {
        int l_tempCounter = m_sendCounter;
        ++m_sendCounter;
        if (m_sendCounter > MAXPACKETNUM) // 패킷 넘버 돌리기
        {
            m_sendCounter = 0;
        }
        return l_tempCounter;
    }
    private bool RecvPacketCountUp(int _counter)
    {
        if (m_recvCounter != _counter)
        {
            return false;
        }
        else
        {
            ++m_recvCounter;
            if (m_recvCounter > MAXPACKETNUM) // 패킷 넘버 돌리기
            {
                m_recvCounter = 0;
            }
            return true;
        }
    }
    public void Write(int _protocol, float _x, float _y)
    {
        byte[] sendBuffer = new byte[32];
        int size = 0;
        // 프로토콜
        size += sizeof(int);
        Buffer.BlockCopy(BitConverter.GetBytes(_protocol), 0, sendBuffer, size, sizeof(int));
        // 패킷넘버
        size += sizeof(int);
        Buffer.BlockCopy(BitConverter.GetBytes(SendPacketCountUp()), 0, sendBuffer, size, sizeof(int));

        // X 좌표
        size += sizeof(float);
        Buffer.BlockCopy(BitConverter.GetBytes(_x), 0, sendBuffer, size, sizeof(float));
        // Y 좌표
        size += sizeof(float);
        Buffer.BlockCopy(BitConverter.GetBytes(_y), 0, sendBuffer, size, sizeof(float));

        // 사이즈 넣기
        Buffer.BlockCopy(BitConverter.GetBytes(size), 0, sendBuffer, 0, sizeof(int));
        _sendQ.Enqueue(sendBuffer);
        autoEvent.Set();
    }
    public void Write(int _protocol)
    {
        byte[] sendBuffer = new byte[32];
        int size = 0;

        // 프로토콜
        size += sizeof(int);
        Buffer.BlockCopy(BitConverter.GetBytes(_protocol), 0, sendBuffer, size, sizeof(int));
        // 패킷넘버
        size += sizeof(int);
        Buffer.BlockCopy(BitConverter.GetBytes(SendPacketCountUp()), 0, sendBuffer, size, sizeof(int));

        // 사이즈 넣기
        Buffer.BlockCopy(BitConverter.GetBytes(size), 0, sendBuffer, 0, sizeof(int));
        _sendQ.Enqueue(sendBuffer);
        autoEvent.Set();
    }

    public bool CheckRead()
    {
        if (_recvQ.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public int GetProtocol()
    {
        byte[] recvBuffer = _recvQ.Peek();
        return BitConverter.ToInt32(recvBuffer, 0);
    }
    public int GetPacketNumber()
    {
        byte[] recvBuffer = _recvQ.Peek();
        return BitConverter.ToInt32(recvBuffer, 4);
    }

    public void GetIdData(out int _id)
    {
        byte[] recvBuffer = _recvQ.Dequeue();
        _id = BitConverter.ToInt32(recvBuffer, 8);
    }

    public void GetInData(out int _id)
    {
        byte[] recvBuffer = _recvQ.Dequeue();
        _id = BitConverter.ToInt32(recvBuffer, 8);
    }
    public void GetMoveData(out int _id, out float _x, out float _y)
    {
        byte[] recvBuffer = _recvQ.Dequeue();
        _id = BitConverter.ToInt32(recvBuffer, 8);
        _x = BitConverter.ToSingle(recvBuffer, 12);
        _y = BitConverter.ToSingle(recvBuffer, 16);
    }
    public void GetOutData(out int _id)
    {
        byte[] recvBuffer = _recvQ.Dequeue();
        _id = BitConverter.ToInt32(recvBuffer, 8);
    }


    private byte[] SubArray(byte[] _data, int _startIndex, int _endIndex)
    {
        int length = _endIndex - _startIndex + 1;

        byte[] result = new byte[length];
        Buffer.BlockCopy(_data, _startIndex, result, 0, length);
        return result;
    }


    #region 암복호화
    private void Encryption(int _dataSize, byte[] _data)
    {
        // size필드 암호화하지 않기위한 offset값
        int l_offset = sizeof(int);
        uint l_tempKey = m_cryptionMainKey;
        byte[] l_tempData = new byte[2048];

        for (int i = l_offset; i < _dataSize; i++)
        {
            l_tempData[i] = (byte)(_data[i] ^ l_tempKey >> 8);
            l_tempKey = (l_tempData[i] + l_tempKey) * m_cryptionSubKey1 + m_cryptionSubKey2;
        }

        Array.Copy(l_tempData, l_offset, _data, l_offset, _dataSize - l_offset);
    }
    private void Decryption(int _dataSize, byte[] _data)
    {
        uint l_tempKey = m_cryptionMainKey;
        byte l_previousBlock;
        byte[] l_tempData = new byte[2048];

        for (int i = 0; i < _dataSize; i++)
        {
            l_previousBlock = _data[i];
            l_tempData[i] = (byte)(_data[i] ^ l_tempKey >> 8);
            l_tempKey = (l_previousBlock + l_tempKey) * m_cryptionSubKey1 + m_cryptionSubKey2;
        }

        Array.Copy(l_tempData, _data, _dataSize);
    }

    public void CryptoKeyDataSetting()
    {
        byte[] l_recvBuffer = _recvQ.Dequeue();

        m_cryptionMainKey = BitConverter.ToUInt32(l_recvBuffer, 8);
        m_cryptionSubKey1 = BitConverter.ToUInt32(l_recvBuffer, 12);
        m_cryptionSubKey2 = BitConverter.ToUInt32(l_recvBuffer, 16);

        m_IsGetKey = true;
    }
    #endregion
}
