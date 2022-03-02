namespace CLUNL.Scripting.SMS
{
    internal enum SMSOperation
    {
        NEW = 0x00,             //Create new object. NEW Object Type [Parameter0 Parameter1 ...]
        SET = 0x01,             //Set value to an object. SET Object Value
        SETF = 0x14,            //Set value to a field of an object. SETF Object Field0 Field1 ... Field[N] Value
        EXEC = 0x02,            //Execute external method. EXEC Object/Type MethodName [Parameter0 Parameter1 ...]
        EXER = 0x13,            //Execute external method and receive return value. EXER Object/Type MethodName WhereToStoreRetureValue [Parameter0 Parameter1 ...]
        IF = 0x03,              //If sentence. ID BOOL_VALUE LABEL
        EQL = 0x15,             //Judge if two object is equal. EQL BoolObj Obj0 Obj1. To implement !=, recommend: EQL BOOL_VALUE BOOL_VALUE Bool:False
        LGR = 0x16,             //Judge if object 1 is larger than object 2. LGR BoolObj Obj1 Obj2. To implement '<', it is recommended to swap the order.
        LGE = 0x17,             //Judge if object 1 is larger than or equal to object 2. LGE BoolObj Obj1 Obj2. This operation is equal to 'BoolObj = Obj1 >= Obj2;'
        J = 0x04,               //Jump to label. J Label_Name
        LABEL = 0x05,           //Define label. LABEL Label_Name
        END = 0x06,             //End of program. END
        ENDLABEL = 0x07,        //End of label. ENDLABEL
        DEL = 0x08,             //Delete object. DEL Object0
        ADD = 0x09,             //Add Object0 = Object1 + Object2.  ADD OBJ0 TYPE OBJ1 OBJ2
        ADDI = 0x0A,            //Add immediately. ADD OBJ0 TYPE OBJ1 NUMBER
        MULT = 0x0B,            //Multiply Object0=Object1*Object2. MULT OBJ0 TYPE OBJ1 OBJ2
        MULTI = 0x0C,           //Multiply immediately. MULT OBJ0 TYPE OBJ1 NUMBER
        DIV = 0x0D,             //Divide Object0=Object1/Object2. DIV OBJ0 TYPE OBJ1 OBJ2
        DIVI = 0x0E,            //Divide immediately. DIVI OBJ0 TYPE OBJ1 NUMBER
        DIVII = 0x0F,           //Divide inversed immediately. DIVI OBJ0 TYPE NUMBER OBJ1
        SW = 0x10,              //Save Word. SW ARRAY_OBJECT INDEX TYPE:DATA(Object)
        ADDW = 0x11,            //Add word to ArrayList. ADDW LIST_OBJECT TYPE:DATA                        
        LW = 0x12,              //Load word. LW ARRAY_OBJECT INDEX TARGET_OBJECT
        NL = 0x18,              //New List
        ND = 0x19,              //New Dictionary
        NEWT = 0x1A,            //Create an instance of Type of certain type. NEWT Object Type.
    }
}
