using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUWBasicTribometer_HVRecip.Controllers
{
    internal class ResultsFileWriter
    {
        private readonly string _filename;

        StreamWriter _streamWriter;

        public ResultsFileWriter(string filename) 
        {
            this._filename = filename;
        }

        public bool PrepareFile(string header = "")
        {
            if (_streamWriter != null) 
                return false;

            _streamWriter = new StreamWriter(_filename);
            _streamWriter.WriteLine(header);
            _streamWriter.WriteLine("Timestamp (s)\tMarker\tFx (N)\tFy (N)\tFz (N)\tTx (Nm)\tTy (Nm)\tTz (Nm)");
            return true;
        }

        public void WriteRow(double timestamp, double[] ftValues, int marker = 0)
        {
            _streamWriter.Write($"{timestamp}\t{marker}\t");
            _streamWriter.Write($"{ftValues[0]}\t");
            _streamWriter.Write($"{ftValues[1]}\t");
            _streamWriter.Write($"{ftValues[2]}\t");
            _streamWriter.Write($"{ftValues[3]}\t");
            _streamWriter.Write($"{ftValues[4]}\t");
            _streamWriter.Write($"{ftValues[5]}\t");
            _streamWriter.WriteLine();
        }


        public void Close()
        {
            _streamWriter.Close();
            _streamWriter = null;
        }
    }
}
