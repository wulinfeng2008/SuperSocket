using System;
using System.Collections.Generic;
using System.Text;
using GiantSoft.SocketServiceCore.Command;
using GiantSoft.FtpService.Storage;

namespace GiantSoft.FtpService.Command
{
	class RNFR : ICommand<FtpSession>
	{
		#region ICommand<FtpSession> Members

		public void Execute(FtpSession session, CommandInfo commandData)
		{
			if (!session.Context.Logged)
				return;

			string filepath = commandData.Param;

			if (string.IsNullOrEmpty(filepath))
			{
				session.SendParameterError();
				return;
			}

			long folderID = 0;

			if (session.FtpServiceProvider.IsExistFile(session.Context, filepath))
			{
				session.Context.RenameFor = filepath;
				session.Context.RenameItemType = ItemType.File;
				session.SendResponse(Resource.RenameForOk_350);
			}
			else if (session.FtpServiceProvider.IsExistFolder(session.Context, filepath, out folderID))
			{
				session.Context.RenameFor = filepath;
				session.Context.RenameItemType = ItemType.Folder;
                session.SendResponse(Resource.RenameForOk_350);
			}
			else
			{
				session.SendResponse(session.Context.Message);
			}
		}

		#endregion
	}
}