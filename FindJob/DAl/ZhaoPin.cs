using System.Collections.Generic;
using FindJob.Model;

namespace FindJob.DAl
{
    public interface IZhaoPin
    {
          List<JobInfo> GetJobList();
    }
}