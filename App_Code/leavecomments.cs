using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using CommonClass;
using System.Web.Script.Services;
using System.IO;
using System.Web.UI.WebControls;
/// <summary>
/// Summary description for employee
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class leavecomments : System.Web.Services.WebService
{

    public leavecomments()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }


    /// <summary>
    /// pendingtask comments
    /// </summary>
    /// <param name="leavetranstionid"></param>
    /// <param name="comments"></param>
    /// <param name="pid"></param>
    /// <returns></returns>

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string insertpendingtaskcommentsandget(string leavetranstionid, string comments, string pid)
    {
        string commentsdetails = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        ////ADO.Net
        //SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();


        SqlParameter[] prm = new SqlParameter[4];
        prm[0] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[1] = new SqlParameter("@commentsdate", da.getcdatetime());
        prm[2] = new SqlParameter("@comment", comments);
        prm[3] = new SqlParameter("@transtionid", leavetranstionid);
        //prm[4] = new SqlParameter("@pid", pid);
        ds = da.GetDataSet("tbl_employeeresigndetailstaskcommentsinsert", prm);
        dt = ds.Tables[0];


        commentsdetails += "<ul>";
        foreach (DataRow row in dt.Rows)
        {

            commentsdetails += "<li><div class='col-lg-12 col-md-12 clear-side-padding col-sm-12 leave-request-comments'>";
            char chcomm = ' ';
            char ch1comm = ' ';
            try
            {
                chcomm = row["FirstName"].ToString().ToCharArray().ElementAt(0);
            }
            catch (Exception ex)
            { }
            try
            {
                ch1comm = row["LastName"].ToString().ToCharArray().ElementAt(0);
            }
            catch (Exception ex)
            { }


            if (row["empimg"].ToString() == string.Empty)
            {

                commentsdetails += "<a><div class='img-circle img-initials' style='background-color: rgb(244, 180, 0);'>" + chcomm + ch1comm + "</div></a>";
            }
            else
            {
                if (File.Exists(Server.MapPath("/employee/emppic/" + row["empimg"].ToString())))
                {
                    commentsdetails += "<a><div class='img-circle img-initials' style='background-color: rgb(244, 180, 0);'><img src='/employee/emppic/" + row["empimg"].ToString() + "' /></div></a>";
                }
                else
                {
                    commentsdetails += "<a><div class='img-circle img-initials' style='background-color: rgb(244, 180, 0);'>" + chcomm + ch1comm + "</div></a>";
                }

            }

            commentsdetails += "<div class='comments-txt'>" + row["comment"].ToString() + "</div></li>";

        }


        commentsdetails += "</ul>";
        return commentsdetails;
    }



    /// <summary>
    /// pendingtaskcomments
    /// </summary>
    /// <param name="leavetranstionid"></param>
    /// <returns></returns>

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string pendingtaskcommentsget(string leavetranstionid)
    {
        string commentsdetails = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        ////ADO.Net
        //SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();


        SqlParameter[] prm = new SqlParameter[1];

        prm[0] = new SqlParameter("@transtionid", leavetranstionid);

        ds = da.GetDataSet("tbl_employeeresigndetailstaskcommentsselect", prm);
        dt = ds.Tables[0];


        commentsdetails += "<ul>";
        foreach (DataRow row in dt.Rows)
        {

            commentsdetails += "<li><div class='col-lg-12 col-md-12 clear-side-padding col-sm-12 leave-request-comments'>";
            char chcomm = ' ';
            char ch1comm = ' ';
            try
            {
                chcomm = row["FirstName"].ToString().ToCharArray().ElementAt(0);
            }
            catch (Exception ex)
            { }
            try
            {
                ch1comm = row["LastName"].ToString().ToCharArray().ElementAt(0);
            }
            catch (Exception ex)
            { }


            if (row["empimg"].ToString() == string.Empty)
            {

                commentsdetails += "<a><div class='img-circle img-initials' style='background-color: rgb(244, 180, 0);'>" + chcomm + ch1comm + "</div></a>";
            }
            else
            {
                if (File.Exists(Server.MapPath("/employee/emppic/" + row["empimg"].ToString())))
                {
                    commentsdetails += "<a><div class='img-circle img-initials' style='background-color: rgb(244, 180, 0);'><img src='/employee/emppic/" + row["empimg"].ToString() + "' /></div></a>";
                }
                else
                {
                    commentsdetails += "<a><div class='img-circle img-initials' style='background-color: rgb(244, 180, 0);'>" + chcomm + ch1comm + "</div></a>";
                }

            }

            commentsdetails += "<div class='comments-txt'>" + row["comment"].ToString() + "</div></li>";

        }


        commentsdetails += "</ul>";
        return commentsdetails;
    }




    /// <summary>
    /// /leave comments
    /// </summary>
    /// <param name="leavetranstionid"></param>
    /// <param name="comments"></param>
    /// <param name="pid"></param>
    /// <returns></returns>

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string  insertleavecommentsandget(string leavetranstionid,string comments,string pid)
    {
       string commentsdetails = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        ////ADO.Net
        //SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();


        SqlParameter[] prm = new SqlParameter[5];
        prm[0] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[1] = new SqlParameter("@entrydate", da.getcdatetime());
        prm[2] = new SqlParameter("@comments", comments);
        prm[3] = new SqlParameter("@leavetranstionid", leavetranstionid);
        prm[4] = new SqlParameter("@pid", pid);        
        ds = da.GetDataSet("tbl_empleavedetailscommentsinsert", prm);
        dt = ds.Tables[0];


        commentsdetails += "<ul>";
        foreach (DataRow row in dt.Rows)
        {

            commentsdetails += "<li><div class='col-lg-12 col-md-12 clear-side-padding col-sm-12 leave-request-comments'>";
            char chcomm = ' ';
            char ch1comm = ' ';
            try
            {
                chcomm = row["FirstName"].ToString().ToCharArray().ElementAt(0);
            }
            catch (Exception ex)
            { }
            try
            {
                ch1comm = row["LastName"].ToString().ToCharArray().ElementAt(0);
            }
            catch (Exception ex)
            { }


            if (row["empimg"].ToString() == string.Empty)
            {

                commentsdetails += "<a><div class='img-circle img-initials' style='background-color: rgb(244, 180, 0);'>" + chcomm + ch1comm + "</div></a>";
            }
            else
            {
                if (File.Exists(Server.MapPath("/employee/emppic/" + row["empimg"].ToString())))
                {
                    commentsdetails += "<a><div class='img-circle img-initials' style='background-color: rgb(244, 180, 0);'><img src='/employee/emppic/" + row["empimg"].ToString() + "' /></div></a>";
                }
                else
                {
                    commentsdetails += "<a><div class='img-circle img-initials' style='background-color: rgb(244, 180, 0);'>" + chcomm + ch1comm + "</div></a>";
                }

            }

            commentsdetails += "<div class='comments-txt'>" + row["comments"].ToString() + "</div></li>";

        }


        commentsdetails += "</ul>";
        return commentsdetails;
    }


    ///od wfh <summary>

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string insertodwfgcommentsandget(string leavetranstionid, string comments, string pid)
    {
        string commentsdetails = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        ////ADO.Net
        //SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();


        SqlParameter[] prm = new SqlParameter[5];
        prm[0] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[1] = new SqlParameter("@entrydate", da.getcdatetime());
        prm[2] = new SqlParameter("@comments", comments);
        prm[3] = new SqlParameter("@leavetranstionid", leavetranstionid);
        prm[4] = new SqlParameter("@pid", pid);
        ds = da.GetDataSet("tbl_empodwfhdetailscommentsinsert", prm);
        dt = ds.Tables[0];


        commentsdetails += "<ul>";
        foreach (DataRow row in dt.Rows)
        {

            commentsdetails += "<li><div class='col-lg-12 col-md-12 clear-side-padding col-sm-12 leave-request-comments'>";
            char chcomm = ' ';
            char ch1comm = ' ';
            try
            {
                chcomm = row["FirstName"].ToString().ToCharArray().ElementAt(0);
            }
            catch (Exception ex)
            { }
            try
            {
                ch1comm = row["LastName"].ToString().ToCharArray().ElementAt(0);
            }
            catch (Exception ex)
            { }


            if (row["empimg"].ToString() == string.Empty)
            {

                commentsdetails += "<a><div class='img-circle img-initials' style='background-color: rgb(244, 180, 0);'>" + chcomm + ch1comm + "</div></a>";
            }
            else
            {
                if (File.Exists(Server.MapPath("/employee/emppic/" + row["empimg"].ToString())))
                {
                    commentsdetails += "<a><div class='img-circle img-initials' style='background-color: rgb(244, 180, 0);'><img src='/employee/emppic/" + row["empimg"].ToString() + "' /></div></a>";
                }
                else
                {
                    commentsdetails += "<a><div class='img-circle img-initials' style='background-color: rgb(244, 180, 0);'>" + chcomm + ch1comm + "</div></a>";
                }

            }

            commentsdetails += "<div class='comments-txt'>" + row["comments"].ToString() + "</div></li>";

        }


        commentsdetails += "</ul>";
        return commentsdetails;
    }
    /// </summary>
    /// <param name="leavetranstionid"></param>
    /// <returns></returns>




    ///Att Log  <summary>

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string inserattlogcommentsandget(string leavetranstionid, string comments, string pid)
    {
        string commentsdetails = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        ////ADO.Net
        //SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();


        SqlParameter[] prm = new SqlParameter[5];
        prm[0] = new SqlParameter("@uid", HttpContext.Current.Request.Cookies["user"]["user_id"].ToString());
        prm[1] = new SqlParameter("@entrydate", da.getcdatetime());
        prm[2] = new SqlParameter("@comments", comments);
        prm[3] = new SqlParameter("@leavetranstionid", leavetranstionid);
        prm[4] = new SqlParameter("@pid", pid);
        ds = da.GetDataSet("tbl_entrylogforapprovalcommentsinsert", prm);
        dt = ds.Tables[0];


        commentsdetails += "<ul>";
        foreach (DataRow row in dt.Rows)
        {

            commentsdetails += "<li><div class='col-lg-12 col-md-12 clear-side-padding col-sm-12 leave-request-comments'>";
            char chcomm = ' ';
            char ch1comm = ' ';
            try
            {
                chcomm = row["FirstName"].ToString().ToCharArray().ElementAt(0);
            }
            catch (Exception ex)
            { }
            try
            {
                ch1comm = row["LastName"].ToString().ToCharArray().ElementAt(0);
            }
            catch (Exception ex)
            { }


            if (row["empimg"].ToString() == string.Empty)
            {

                commentsdetails += "<a><div class='img-circle img-initials' style='background-color: rgb(244, 180, 0);'>" + chcomm + ch1comm + "</div></a>";
            }
            else
            {
                if (File.Exists(Server.MapPath("/employee/emppic/" + row["empimg"].ToString())))
                {
                    commentsdetails += "<a><div class='img-circle img-initials' style='background-color: rgb(244, 180, 0);'><img src='/employee/emppic/" + row["empimg"].ToString() + "' /></div></a>";
                }
                else
                {
                    commentsdetails += "<a><div class='img-circle img-initials' style='background-color: rgb(244, 180, 0);'>" + chcomm + ch1comm + "</div></a>";
                }

            }

            commentsdetails += "<div class='comments-txt'>" + row["comments"].ToString() + "</div></li>";

        }


        commentsdetails += "</ul>";
        return commentsdetails;
    }







    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string attlogcommentsget(string leavetranstionid)
    {
        string commentsdetails = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        ////ADO.Net
        //SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();


        SqlParameter[] prm = new SqlParameter[1];

        prm[0] = new SqlParameter("@leavetranstionid", leavetranstionid);

        ds = da.GetDataSet("tbl_entrylogforapprovalcommentsselect", prm);
        dt = ds.Tables[0];


        commentsdetails += "<ul>";
        foreach (DataRow row in dt.Rows)
        {

            commentsdetails += "<li><div class='col-lg-12 col-md-12 clear-side-padding col-sm-12 leave-request-comments'>";
            char chcomm = ' ';
            char ch1comm = ' ';
            try
            {
                chcomm = row["FirstName"].ToString().ToCharArray().ElementAt(0);
            }
            catch (Exception ex)
            { }
            try
            {
                ch1comm = row["LastName"].ToString().ToCharArray().ElementAt(0);
            }
            catch (Exception ex)
            { }


            if (row["empimg"].ToString() == string.Empty)
            {

                commentsdetails += "<a><div class='img-circle img-initials' style='background-color: rgb(244, 180, 0);'>" + chcomm + ch1comm + "</div></a>";
            }
            else
            {
                if (File.Exists(Server.MapPath("/employee/emppic/" + row["empimg"].ToString())))
                {
                    commentsdetails += "<a><div class='img-circle img-initials' style='background-color: rgb(244, 180, 0);'><img src='/employee/emppic/" + row["empimg"].ToString() + "' /></div></a>";
                }
                else
                {
                    commentsdetails += "<a><div class='img-circle img-initials' style='background-color: rgb(244, 180, 0);'>" + chcomm + ch1comm + "</div></a>";
                }

            }

            commentsdetails += "<div class='comments-txt'>" + row["comments"].ToString() + "</div></li>";

        }


        commentsdetails += "</ul>";
        return commentsdetails;
    }

    protected string showdetails(string id)
    {
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        string retstr = "";
        function fns = new function();
        string strsql = "select * from tbl_entrylogforapprovaldatadetails where  timeid='" + id + "'   and status=1 and archive=0 order by datefrom ";
        DataSet dsdetails = da.GetDataSetInline(strsql);

        int count = 0;
        count = dsdetails.Tables[0].Rows.Count * 2;
        if (dsdetails.Tables[0].Rows.Count > 0)
        {
            //retstr = "<span><i class='fa fa-long-arrow-down green'></i>" + Convert.ToDateTime(dsdetails.Tables[0].Rows[0]["datefrom"].ToString()).ToString("hh:mm tt") + "<span> - Requested To Add</span></span><small>&amp; " + (count - 1) + " more </small>";

            retstr += "<label><strong>" + fns.getltext("Attendance Adjustment", "2") + "</strong></label>";
            retstr += "<div class='row'>";
            for (int i = 0; i < dsdetails.Tables[0].Rows.Count; i++)
            {
               
                retstr += "<div class='col-sm-4'>";
                retstr += "<span><i class='fa fa-long-arrow-down green'></i> " + Convert.ToDateTime(dsdetails.Tables[0].Rows[i]["datefrom"].ToString()).ToString("hh:mm tt") + "</span>";
                retstr += "</div>";
                retstr += "<div class='col-sm-8'>";
                retstr += " <span class='red'><i class='fa fa-long-arrow-down green'></i> " + fns.getltext("Requested To Add", "2") + "</span>";
                retstr += " </div>";
                if (dsdetails.Tables[0].Rows[i]["dateto"].ToString() != string.Empty)
                {
                    retstr += "<div class='col-sm-4'>";
                    retstr += "  <span><i class='fa fa-long-arrow-up red'></i> " + Convert.ToDateTime(dsdetails.Tables[0].Rows[i]["dateto"].ToString()).ToString("hh:mm tt") + "</span>";
                    retstr += " </div>";

                    retstr += " <div class='col-sm-8'>";
                    retstr += "  <span class='red'><i class='fa fa-long-arrow-up red'></i> " + fns.getltext("Requested To Add", "2") + "</span>";
                    retstr += " </div>";
                }
            }
            retstr += " </div>";
        }

        strsql = "select * from tbl_entrylogforapprovaldatadetails where timeid='" + id + "' and typeid=3 and status=1 and archive=0 order by datefrom ";
        dsdetails = da.GetDataSetInline(strsql);


        count += dsdetails.Tables[0].Rows.Count * 2;
        if (dsdetails.Tables[0].Rows.Count > 0)
        {
            //retstr = "<span><i class='fa fa-long-arrow-down green'></i>" + Convert.ToDateTime(dsdetails.Tables[0].Rows[0]["datefrom"].ToString()).ToString("hh:mm tt") + "<span> - Requested To Add</span></span><small>&amp; " + (count - 1) + " more </small>";

            retstr += "<label><strong>" + fns.getltext("On Duty", "2") + "</strong></label>";
            retstr += "<div class='row'>";
            for (int i = 0; i < dsdetails.Tables[0].Rows.Count; i++)
            {

                retstr += dsdetails.Tables[0].Rows[i]["updatemessage"].ToString();
            }





            retstr += " </div>";
        }
        strsql = "select * from tbl_entrylogforapprovaldatadetails where timeid='" + id + "' and typeid=4 and status=1 and archive=0 order by datefrom ";
        dsdetails = da.GetDataSetInline(strsql);


        count += dsdetails.Tables[0].Rows.Count * 2;
        if (dsdetails.Tables[0].Rows.Count > 0)
        {
            //retstr = "<span><i class='fa fa-long-arrow-down green'></i>" + Convert.ToDateTime(dsdetails.Tables[0].Rows[0]["datefrom"].ToString()).ToString("hh:mm tt") + "<span> - Requested To Add</span></span><small>&amp; " + (count - 1) + " more </small>";

            retstr += "<label><strong>" + fns.getltext("WFH", "2") + "</strong></label>";
            retstr += "<div class='row'>";
            for (int i = 0; i < dsdetails.Tables[0].Rows.Count; i++)
            {

                retstr += dsdetails.Tables[0].Rows[i]["updatemessage"].ToString();
            }





            retstr += " </div>";
        }
        return retstr;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string stringattlogselect(string leavetranstionid)
    {
        string commentsdetails = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
            function fns = new function();
    ////ADO.Net
    //SqlConnection cn = new SqlConnection();
    DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"].ToString());         
        prm[1] = new SqlParameter("@id", leavetranstionid);       
        ds = da.GetDataSet("tbl_entrylogforapprovalselect", prm);
        dt = ds.Tables[0];
        if(ds.Tables[0].Rows.Count>0)
        {
            commentsdetails += ds.Tables[0].Rows[0]["empname"].ToString() + "#####" + Convert.ToDateTime(ds.Tables[0].Rows[0]["attdate"].ToString()).ToString("MMM dd, yyyy");
            commentsdetails += "#####" + fns.getltext("Attendance Adjustment", "2") + "";
            commentsdetails += "#####" + ds.Tables[0].Rows[0]["notes"].ToString();
            commentsdetails += "#####" + showdetails(ds.Tables[0].Rows[0]["id"].ToString());
            commentsdetails += "#####" + attlogcommentsget(ds.Tables[0].Rows[0]["id"].ToString());
            
        }

        return commentsdetails;
    }





    /// </summary>
    /// <param name="leavetranstionid"></param>
    /// <returns></returns>















       


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string leavecommentsget(string leavetranstionid)
    {
        string commentsdetails = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        ////ADO.Net
        //SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();


        SqlParameter[] prm = new SqlParameter[1];
       
        prm[0] = new SqlParameter("@leavetranstionid", leavetranstionid);
    
        ds = da.GetDataSet("tbl_empleavedetailscommentsselect", prm);
        dt = ds.Tables[0];


        commentsdetails += "<ul>";
        foreach (DataRow row in dt.Rows)
        {

            commentsdetails += "<li><div class='col-lg-12 col-md-12 clear-side-padding col-sm-12 leave-request-comments'>";
            char chcomm = ' ';
            char ch1comm = ' ';
            try
            {
                chcomm = row["FirstName"].ToString().ToCharArray().ElementAt(0);
            }
            catch (Exception ex)
            { }
            try
            {
                ch1comm = row["LastName"].ToString().ToCharArray().ElementAt(0);
            }
            catch (Exception ex)
            { }


            if (row["empimg"].ToString() == string.Empty)
            {

                commentsdetails += "<a><div class='img-circle img-initials' style='background-color: rgb(244, 180, 0);'>" + chcomm + ch1comm + "</div></a>";
            }
            else
            {
                if (File.Exists(Server.MapPath("/employee/emppic/" + row["empimg"].ToString())))
                {
                    commentsdetails += "<a><div class='img-circle img-initials' style='background-color: rgb(244, 180, 0);'><img src='/employee/emppic/" + row["empimg"].ToString() + "' /></div></a>";
                }
                else
                {
                    commentsdetails += "<a><div class='img-circle img-initials' style='background-color: rgb(244, 180, 0);'>" + chcomm + ch1comm + "</div></a>";
                }

            }

            commentsdetails += "<div class='comments-txt'>" + row["comments"].ToString() + "</div></li>";

        }


        commentsdetails += "</ul>";
        return commentsdetails;
    }



    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string onholdcommentsget(string empid, string mid, string yearid)
    {
        string commentsdetails = "";
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        ////ADO.Net
        //SqlConnection cn = new SqlConnection();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();


        SqlParameter[] prm = new SqlParameter[3];

        prm[0] = new SqlParameter("@uid", empid);
        prm[2] = new SqlParameter("@mid", mid);
        prm[1] = new SqlParameter("@yearid", yearid);

        ds = da.GetDataSet("tbl_payrollonholdcommentsselect", prm);
        dt = ds.Tables[0];


        commentsdetails += "<ul>";
        foreach (DataRow row in dt.Rows)
        {

            commentsdetails += "<li><div class='col-lg-12 col-md-12 clear-side-padding col-sm-12 leave-request-comments'>";
            char chcomm = ' ';
            char ch1comm = ' ';
            try
            {
                chcomm = row["FirstName"].ToString().ToCharArray().ElementAt(0);
            }
            catch (Exception ex)
            { }
            try
            {
                ch1comm = row["LastName"].ToString().ToCharArray().ElementAt(0);
            }
            catch (Exception ex)
            { }


            if (row["empimg"].ToString() == string.Empty)
            {

                commentsdetails += "<a><div class='img-circle img-initials' style='background-color: rgb(244, 180, 0);'>" + chcomm + ch1comm + "</div></a>";
            }
            else
            {
                if (File.Exists(Server.MapPath("/employee/emppic/" + row["empimg"].ToString())))
                {
                    commentsdetails += "<a><div class='img-circle img-initials' style='background-color: rgb(244, 180, 0);'><img src='/employee/emppic/" + row["empimg"].ToString() + "' /></div></a>";
                }
                else
                {
                    commentsdetails += "<a><div class='img-circle img-initials' style='background-color: rgb(244, 180, 0);'>" + chcomm + ch1comm + "</div></a>";
                }

            }

            commentsdetails += "<div class='comments-txt'><b>" + row["DisplayName"].ToString() + "</b><br/>" + row["remarks"].ToString() + "<br/>" + Convert.ToDateTime(row["cdate"].ToString()).ToString("MMM ,dd yyyy hh:mm tt") + " </div></li>";

        }


        commentsdetails += "</ul>";
        return commentsdetails;
    }


}
