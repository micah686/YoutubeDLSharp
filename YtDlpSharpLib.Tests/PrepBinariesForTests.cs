using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YtDlpSharpLib.Tests
{
    [TestClass]
    public class PrepBinariesForTests
    {
        private static bool _didDownloadBinaries = false;

        [AssemblyInitialize]
        public static void DownloadBinaries(TestContext context)
        {
            if (_didDownloadBinaries == false)
            {
                if (!File.Exists(Utils.YtDlpBinaryName()))
                {
                    Utils.DownloadYtDlp();
                }
                if (!File.Exists(Utils.FfmpegBinaryName()))
                {
                    Utils.DownloadFFmpeg();
                }
                if (!File.Exists(Utils.FfprobeBinaryName()))
                {
                    Utils.DownloadFFprobe();
                }
                _didDownloadBinaries = true;
                Console.WriteLine(_didDownloadBinaries);
            }
        }
    }
}
