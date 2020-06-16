using CLUNL.Data.Layer0;
using CLUNL.DirectedIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CLUNL.Data.Layer1
{
    public class INILikeData : BasicKeyValueData
    {
        internal INILikeData(IBaseWR WR,bool AutoLoad=true) : base(WR,'=',AutoLoad) {

        }
        public static INILikeData CreateToWR(IBaseWR WR)
        {
            WR.Position = 0;
            WR.SetLength(0);
            WR.WriteLine("#=================================================");
            WR.WriteLine("#=INI-Like Data File                             =");
            WR.WriteLine("#=Created by CLUNL.Data.Layer1.INILikeData       =");
            WR.WriteLine("#=================================================");
            WR.Flush();
            INILikeData _ω___ = new INILikeData(WR, true);
            return _ω___;
        }
        public static INILikeData CreateToFile(FileInfo TargetFile)
        {
            FileWR _α___ = new FileWR(TargetFile);
            _α___.WriteLine("#=================================================");
            _α___.WriteLine("#=INI-Like Data File                             =");
            _α___.WriteLine("#=Created by CLUNL.Data.Layer1.INILikeData       =");
            _α___.WriteLine("#=================================================");
            _α___.Flush();
            INILikeData __＿ = new INILikeData(_α___);
            return __＿;
        }
        public static INILikeData LoadFromStream(Stream stream)
        {
            INILikeData _ω___ = new INILikeData(new StreamWR(stream), true);
            return _ω___;
        }
        public static INILikeData LoadFromWR(IBaseWR WR)
        {
            INILikeData _ω___ = new INILikeData(WR, true);
            return _ω___;
        }
    }
}
