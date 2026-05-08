using CoreServiceLib.Core.IocExt;
using CoreServiceLib.Interfaces.ManulControl;
using CoreServiceLib.Models.ManulInfo;
using CoreServiceLib.Services.Manul;

using Newtonsoft.Json.Linq;

namespace CoreServiceLib.Bases
{
    public abstract class ManulBase : IManul
    {
        public abstract int ManulTypeID { get; }

        public abstract string ManulControlName { get; }

        public abstract string DiagUIName { get; }

        public virtual int MannulDo(JObject mPara)
        {
            return -1;
        }


        public ManulPara<Title, InputPara, OutPutPara> GetManulPara<Title, InputPara, OutPutPara>(ManulControlConfig mcfg)
        {
            var para = new ManulPara<Title, InputPara, OutPutPara>
            {
                MID = mcfg.MID,
                ManulTypeID = mcfg.ManulTypeID,
                ManulControlName = mcfg.ManulControlName,
                RowNumber = mcfg.RowNumber,
                ColNumber = mcfg.ColNumber,
                DeviceId = mcfg.DeviceId,
                ManulActionPara = mcfg.ManulActionPara.ToObject<InputPara>(),
                ManulTitles = mcfg.ManulTitles.ToObject<Title>(),
                OutputParas = mcfg.OutputParas.ToObject<OutPutPara>(),
                PageNumber = mcfg.PageNumber,
                IsDisable = mcfg.IsDisable,
            };
            return para;
        }


        public ManulControlConfig SaveManulPara<Title, InputPara, OutPutPara>(ManulPara<Title, InputPara, OutPutPara> mPara)
        {
            var mcfg = new ManulControlConfig
            {
                MID = mPara.MID,
                ManulTypeID = mPara.ManulTypeID,
                ColNumber = mPara.ColNumber,
                DeviceId = mPara.DeviceId,
                IsDisable = mPara.IsDisable,
                ManulControlName = mPara.ManulControlName,
                PageNumber = mPara.PageNumber,
                RowNumber = mPara.RowNumber,
                ManulActionPara = JObject.FromObject(mPara.ManulActionPara),
                OutputParas = JObject.FromObject(mPara.OutputParas),
                ManulTitles = JObject.FromObject(mPara.ManulTitles)
            };
            return mcfg;
        }

        public JObject ManulConfigUI(JObject mcfg)
        {
            mcfg ??= [];
            var dig = McsApp.GetDiag(DiagUIName, mcfg);
            dig.ShowDialogUI();
            return dig.ReturnData<JObject>();
        }
    }
}
