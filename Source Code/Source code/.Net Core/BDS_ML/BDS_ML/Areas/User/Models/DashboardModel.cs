using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDS_ML.Areas.User.Models
{
    public class Dashboard
    {
        public Dashboard()
        {

        }
    private int postNumber;
    private int postPending;
    private int postSoldNumber;
    private int postfollowed;

        public int PostNumber { get => postNumber; set => postNumber = value; }
        public int PostPending { get => postPending; set => postPending = value; }
        public int PostSoldNumber { get => postSoldNumber; set => postSoldNumber = value; }
        public int Postfollowed { get => postfollowed; set => postfollowed = value; }
    }
}
