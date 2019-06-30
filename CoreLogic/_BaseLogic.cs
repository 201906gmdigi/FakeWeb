using System;
using System.Diagnostics;
using CoreLogic.Dto;
using CoreWebCommon;
using CoreWebCommon.Dto;
using NLog;

namespace CoreLogic
{
    public abstract class _BaseLogic : IDisposable
    {
        private readonly Operation _operation;

        protected _BaseLogic(Operation operation)
        {
            _operation = operation;
        }

        protected virtual IMyLogger GetLogger()
        {
            return new NLogAdapter();
        }

        protected OperationInfo GetOperationInfo()
        {
            return new OperationInfo
            {
                Display = _operation.Display,
                IP = _operation.IP,
                UserId = _operation.UserId
            };
        }

        public void Dispose()
        {
        }
    }

    public class NLogAdapter : IMyLogger
    {
        private readonly Logger _nLogLogger;

        public NLogAdapter()
        {
            _nLogLogger = GetNLogLogger();
        }

        private static Logger GetNLogLogger()
        {
            var fullMethodName = default(string);

            try
            {
                var method = new StackFrame(1).GetMethod();
                fullMethodName = $"{method.DeclaringType.FullName}.{method.Name}";
            }
            catch (Exception ex)
            {
                fullMethodName = ex.GetBaseException().Message;
            }

            return LogManager.GetLogger(fullMethodName);
        }

        public void Info(string message)
        {
            _nLogLogger.Info(message);
        }
    }
}