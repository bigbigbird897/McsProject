using CoreServiceLib.Core.Extensions.McsLogger;
using CoreServiceLib.Core.McsEvent;

using NLog;

namespace CoreWpfLib.WpfEx
{
    public static class LoggerExt
    {
        public static void McsLogInfo(this ILogger logger, string message)
        {
            logger.Info(message);
            ToDisplay(message, logger.Name, LoggerInfoType.Info);
        }

        public static void McsLogErr(this ILogger logger, Exception ex)
        {
            logger.Error(ex);
            McsEventMessage.Instance.Publish(new SysErrMessage { MessageInfo = ex.Message, ModeName = logger.Name });
        }
        public static void McsLogErr(this ILogger logger, string errInfo)
        {
            logger.Error(errInfo);
            McsEventMessage.Instance.Publish(new SysErrMessage { MessageInfo = errInfo, ModeName = logger.Name });
        }

        public static void McsLogWarn(this ILogger logger, string message)
        {
            logger.Warn(message);
            ToDisplay(message, logger.Name, LoggerInfoType.warning);
        }

        public static void McsLogTrace(this ILogger logger, string message)
        {
            logger.Trace(message);
            ToDisplay(message, logger.Name, LoggerInfoType.Info);
        }

        public static void McsLogDebug(this ILogger logger, string message)
        {
            logger.Debug(message);
            ToDisplay(message, logger.Name, LoggerInfoType.Info);
        }

        private static void ToDisplay(string message, string modeName, LoggerInfoType infoType)
        {
            var mSysMessage = new SysMessage
            {
                ModeName = modeName,
                MType = infoType,
                MessageInfo = message,
                OccurredTime = DateTime.Now
            };

            LogBuffer.SendBuffer(mSysMessage);
            McsEventMessage.Instance.Publish(LogBuffer.GetBufferData());
        }
    }
}