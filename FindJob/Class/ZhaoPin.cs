using System.Collections.Generic;
using FindJob.Model;

namespace FindJob.Class
{
    public interface IZhaoPin
    {
          List<JobInfo> GetJobList();
    }
}