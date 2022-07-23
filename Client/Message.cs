using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Message
    {
        private byte opcode;
        private ushort length;
        private byte[] payload;

        public byte Opcode {
            get {
                return opcode;
            }

            set {
                opcode = value;
            }
        }

        public ushort Length {
            get {
                return length;
            }

            set {
                length = value;
            }
        }

        public byte[] Payload {
            get {
                return payload;
            }

            set {
                payload = value;
            }
        }

        public Message(byte[] recvBuff)
        {
            this.Opcode = recvBuff[0];
            this.Length = BitConverter.ToUInt16(recvBuff, Constants.OPCODE_SIZE);
            this.Payload = new byte[Constants.BUFFER_SIZE];
            if(this.Length > 0)
            {
                Array.Copy(recvBuff, Constants.OPCODE_SIZE + Constants.LENGTH_SIZE, this.Payload, 0, this.Length);
                this.Payload[this.length] = 0;
            }
        }

        public Message(byte opcode, ushort length, string name)
        {
            this.Opcode = opcode;
            this.Length = length;
            this.Payload = new byte[Constants.BUFFER_SIZE - Constants.OPCODE_SIZE - Constants.LENGTH_SIZE];
            Array.Copy(Encoding.ASCII.GetBytes(name), 0, this.Payload, 0, this.Length);
        }

        public Message(byte opcode)
        {
            this.Opcode = opcode;
            this.Length = 0;
            this.Payload = new byte[1];
            this.Payload[0] = 0;
        }


        public Message(byte opcode, ushort length, byte locationX, byte locationY) {
            this.Opcode = opcode;
            this.Length = length;
            this.Payload = new byte[] { locationX, locationY }; 

        }

        //@funtion convertToString: convertthe message object to string
        //@return result
        public string convertToString() {
            string result = Convert.ToString(this.Opcode) + Convert.ToString(this.Length) + Convert.ToString(this.Payload);
            return result;
        }
    }
}
