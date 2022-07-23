namespace Client
{
    public class Constants
    {
        public const string APP_NAME = "TicTacToe!";
        public static int BUTTON_WIDTH = 20;
        public static int BUTTON_HEIGHT = 20;
        public static int BOARD_SIZE = 16;

        public static int OPCODE_SIZE = 1;
        public static int LENGTH_SIZE = 2;
        public static int LOCATION_SIZE = 1;
        public static string SAMPLE_0000 = "0000";
        public static string SAMPLE_00 = "00";
        public static string SPACE = " ";

        public static string IP = "127.0.0.1";
        public static int port = 5500;
        public static int BUFFER_SIZE = 1024;


        public const int FORM_MAIN = 0;
        public const int FORM_PLAY = 1;
        public const int FORM_ACCOUNT = 2;

        public const int TURN_O = 1;
        public const int TURN_X = 2;

        /* Opcode */

        public const byte OPCODE_FILE = 0;
        public const byte OPCODE_FILE_DATA =  1;
        public const byte OPCODE_FILE_ERROR =  2;

        public const byte OPCODE_SIGN_UP =  10;
        public const byte OPCODE_SIGN_UP_SUCESS =  11;
        public const byte OPCODE_SIGN_UP_DUPLICATED_USERNAME =  12;
        public const byte OPCODE_SIGN_UP_INVALID_USERNAME =  13;
        public const byte OPCODE_SIGN_UP_INVALID_PASSWORD =  14;
        public const byte OPCODE_SIGN_UP_UNKNOWN_ERROR =  19;

        public const byte OPCODE_SIGN_IN = 20;
        public const byte OPCODE_SIGN_IN_SUCESS = 21;
        public const byte OPCODE_SIGN_IN_ALREADY_LOGGED_IN = 22;
        public const byte OPCODE_SIGN_IN_USERNAME_NOT_FOUND = 23;
        public const byte OPCODE_SIGN_IN_INVALID_USERNAME = 24;
        public const byte OPCODE_SIGN_IN_INVALID_PASSWORD = 25;
        public const byte OPCODE_SIGN_IN_WRONG_PASSWORD = 26;
        public const byte OPCODE_SIGN_IN_UNKNOWN_ERROR = 29;

        public const byte OPCODE_SIGN_OUT = 30;
        public const byte OPCODE_SIGN_OUT_SUCCESS = 31;
        public const byte OPCODE_SIGN_OUT_NOT_LOGGED_IN = 32;
        public const byte OPCODE_SIGN_OUT_ERROR_UNKNOWN = 39;

        public const byte OPCODE_LIST = 40;
        public const byte OPCODE_LIST_REPLY = 41;

        public const byte OPCODE_CHALLENGE = 50;
        public const byte OPCODE_CHALLENGE_ACCEPT = 51;
        public const byte OPCODE_CHALLENGE_REFUSE = 52;
        public const byte OPCODE_CHALLENGE_INVALID_RANK = 53;
        public const byte OPCODE_CHALLENGE_BUSY = 54;
        public const byte OPCODE_CHALLENGE_NOT_FOUND = 55;

        public const byte OPCODE_INFO = 60;
        public const byte OPCODE_INFO_FOUND = 61;
        public const byte OPCODE_INFO_NOT_FOUND = 62;

        public const byte OPCODE_PLAY = 70;
        public const byte OPCODE_PLAY_OPPONENT = 71;
        public const byte OPCODE_PLAY_INVALID_CORDINATE = 72;
        public const byte OPCODE_PLAY_INVALID_TURN = 73;
        public const byte OPCODE_SURRENDER = 80;
        public const byte OPCODE_SURRENDER_NO_ROOM = 81;

        public const byte OPCODE_RESULT = 90;
    }
}
