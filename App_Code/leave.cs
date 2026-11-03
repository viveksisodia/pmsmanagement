using System;
using System.Data.SqlClient;
using System.Collections;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CommonClass;

/// <summary>
/// Summary description for leave
/// </summary>
public class leave
{
    DataAccess dataaccess;
    SqlParameter[] param;
    DataSet ds;
    DataAccess da = new DataAccess();
    CommonFunction cf = new CommonFunction();
    //public leave()
    //{


    //}

    public string getemp_code(string uid)
    {
        dataaccess = new DataAccess();
        string sql = "select empcode as EMP_CODE from tbl_employee where id=" + uid;
        string retstr = dataaccess.getexecutescalar(sql);
        return retstr;

    }

    public string getshiftid(string uid)
    {
        dataaccess = new DataAccess();
        string sql = "select ShiftType from tbl_employee where id=" + uid;
        string retstr = dataaccess.getexecutescalar(sql);
        return retstr;

    }


    public string getattcaptureid(string uid)
    {
        dataaccess = new DataAccess();
        string sql = "select AttendanceCaptureScheme from tbl_employee where id=" + uid;
        string retstr = dataaccess.getexecutescalar(sql);
        return retstr;

    }
    public string getemp_attno(string uid)
    {
        dataaccess = new DataAccess();
        string sql = "select EmpNo as AttendanceNumber from tbl_employee where id=" + uid;
        string retstr = dataaccess.getexecutescalar(sql);
        return retstr;

    }
    public string getemp_intime(string uid,string date)
    {
        dataaccess = new DataAccess();
        string sql = "select top 1 datefrom from tbl_entrylogforapprovaldatadetails where empid=" + uid+ " and cast(punchdate as date)=cast('" + date+ "' as date) order by datefrom ";
        string retstr = dataaccess.getexecutescalar(sql);
        return retstr;

    }
    public string getempisrosterid(string uid, string date)
    {
        dataaccess = new DataAccess();
        string strsqlroster = "select id from tbl_employee where id='" + uid + "' and isnull(isrosterbase,0)='1'";
        DataSet dsroster = da.GetDataSetInline(strsqlroster);
        if(dsroster.Tables[0].Rows.Count>0)
        {
            string strrostershift = "select * from tbl_employeerosterdetails where uid='" + uid + "' and status=1 and archive=0  and cast(rdate as date)=cast('" + date + "' as date)";
            DataSet ds = da.GetDataSetInline(strrostershift);
            if(ds.Tables[0].Rows.Count>0)
            {
                return ds.Tables[0].Rows[0]["shiftid"].ToString();
            }

        }

        
        return "0";

    }
    public string getemp_outtime(string uid, string date)
    {
        string retstr = "";
        try
        {
            dataaccess = new DataAccess();
            string sql = "select top 1 dateto from tbl_entrylogforapprovaldatadetails where empid=" + uid + " and cast(punchdate as date)=cast('" + date + "' as date) and dateto is not null order by dateto desc ";
             retstr = dataaccess.getexecutescalar(sql);
        }
        catch
        {

        }
        return retstr;

    }
    public string getemp_idbycode(string uid)
    {
        dataaccess = new DataAccess();
        string sql = "select id as EMPid from tbl_employee where empcode like '" + uid+"'";
        try
        {
            string retstr = dataaccess.getexecutescalar(sql);
            return retstr;
        }
        catch (Exception)
        {
            return "0";
        }

    }
    public double getleftleave(int uid, int leave_type)
    {
        dataaccess = new DataAccess();
        double retint;
        param = new SqlParameter[2];
        param[0] = new SqlParameter("@uid", uid);
        param[1] = new SqlParameter("@leave_type", leave_type);
        ds = dataaccess.GetDataSet("sm_fatchleftleave_half", param);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Convert.ToString(ds.Tables[0].Rows[0]["leftleave"]) != "")
                retint = Convert.ToDouble(Convert.ToString(ds.Tables[0].Rows[0]["leftleave"]));
            else
                retint = 0;

        }
        else
        {
            retint = 0;
        }
        return retint;
    }

    public string getlastdateofthemonth(string mdate)
    {
        dataaccess = new DataAccess();
        string strsql = "select [dbo].[getlastdayofthemonth]('" + mdate + "') as ldate";
        string retstr = dataaccess.getexecutescalar(strsql);
        return retstr;

    }


    public string gettotalpaidday(string mdate, string empcod, string days, string empid)
    {
        dataaccesshr dahr = new dataaccesshr();
        string retstr = "";
        decimal totalpaid = 0;
        decimal totalAbsent = 0;
        decimal totalpresent = 0;
        decimal totalpresentshow = 0;
        int totallate = 0;
        decimal totalHalfDay = 0;
        decimal totalHalfDayshow = 0;
        decimal totalHalfDayapprove = 0;
        decimal totalHalfDayunapprove = 0;
        decimal totalHoliday = 0;
        decimal totalWeekOff = 0;
        decimal LeaveunApproved = 0;
        decimal LeaveunApprovedshow = 0;
        decimal LeaveApproved = 0;
        decimal totalleaveadc = 0;
        decimal odApproved = 0;
        decimal odunApproved = 0;
        decimal totalod = 0;
        decimal totalunod = 0;
        decimal halfodApproved = 0;
        decimal halfodunApproved = 0;
        string strssss = "";
        decimal totalatecommingshow = 0;

        decimal totalpresentshowCompOff = 0;
        decimal totalpresentCompOff = 0;
        decimal totalHalfDayCompOff = 0;


        string strsqltotalmonth = "select * from [dbo].[gettotaldate]('" + mdate + "','" + getlastdateofthemonth(mdate) + "')";

        //HttpContext.Current.Response.Write(strsqltotalmonth);


        DataSet dsdate = da.GetDataSetInline(strsqltotalmonth);
        if (dsdate.Tables[0].Rows.Count > 0)
        {



            int sleave = 0;


            for (int idate = 0; idate < dsdate.Tables[0].Rows.Count; idate++)
            {


                SqlParameter[] prm = new SqlParameter[4];
                prm[0] = new SqlParameter("@fromDate", dsdate.Tables[0].Rows[idate]["date"]);
                prm[1] = new SqlParameter("@toDate", dsdate.Tables[0].Rows[idate]["date"]);
                prm[2] = new SqlParameter("@empCode", empcod);
                prm[3] = new SqlParameter("@cid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                DataSet ds = dahr.GetDataSet("AdminSearchPunchReport", prm);

                //SqlParameter[] prm = new SqlParameter[3];
                //prm[0] = new SqlParameter("@fromDate", mdate);
                //prm[1] = new SqlParameter("@toDate", getlastdateofthemonth(mdate));
                //prm[2] = new SqlParameter("@empCode", empcod);
                //DataSet ds = dahr.GetDataSet("AdminSearchPunchReport", prm);


                string dateoffice = dsdate.Tables[0].Rows[idate]["date"].ToString();
                string dateIN1 = "";
                string dateOUT2 = "";
                string totalworkingho = "";
                string totalworkinghoh = "";

                if (ds.Tables[0].Rows.Count > 0)
                {
                    dateoffice = ds.Tables[0].Rows[0]["DateOFFICE"].ToString();
                    dateIN1 = ds.Tables[0].Rows[0]["IN1"].ToString();
                    dateOUT2 = ds.Tables[0].Rows[0]["OUT2"].ToString();
                    totalworkingho = ds.Tables[0].Rows[0]["WorkHr"].ToString();
                    //try
                    //{
                    string[] strwr = totalworkingho.Split(' ');
                    if (strwr.Length > 1)
                    {
                        totalworkinghoh = strwr[0];
                    }
                    //}
                    //catch (Exception)
                    //{

                    //}

                }

                try
                {
                    double tttw = 0;
                    if (totalworkinghoh != string.Empty)
                        tttw = Convert.ToDouble(totalworkinghoh);


                }
                catch (Exception ex)
                {
                    totalworkinghoh = "0";
                }

                string strstatus = empcod + " " + mdate + " " + getlastdateofthemonth(mdate) + "<br/>";
                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{


                strstatus = GetStatus(dateoffice, dateIN1, dateOUT2, empid);

                //HttpContext.Current.Response.Write("ss "+totalworkinghoh + " " + idate +"hi ");

                double totalworkhourafterworking = 0;
                string totalworkafterworking_status = "";

                try
                {

                    // if (totalworkinghoh != string.Empty && (strstatus == "Present" || strstatus == "PresentWO"))

                    if (totalworkinghoh != string.Empty && (strstatus == "Present"))
                    {
                        if (Convert.ToDouble(totalworkinghoh) < 9)
                        {
                            if (Convert.ToDouble(totalworkinghoh) < 4.30)
                            {

                                strstatus = "Absent";
                            }
                            else if (Convert.ToDouble(totalworkinghoh) >= 8.30)
                            {
                                if (sleave < 2)
                                {
                                    sleave++;
                                    strstatus = "Present";
                                }
                                else
                                {
                                    strstatus = "HalfDay";
                                }
                            }
                            else
                            {
                                strstatus = "HalfDay";
                            }
                        }
                        ////else
                        ////{
                        ////    strstatus = GetStatus(dateoffice, dateIN1, dateOUT2, empid);
                        ////}
                        //totalworkhourafterworking = Convert.ToDouble(Convert.ToDouble(totalworkinghoh) - 9);

                        //if (Convert.ToDouble(totalworkhourafterworking) > 8)
                        //{
                        //    totalworkafterworking_status = "Present(COMPOFF)";
                        //}
                        //else if (Convert.ToDouble(totalworkhourafterworking) > 4)
                        //{
                        //    totalworkafterworking_status = "HalfDay(COMPOFF)";
                        //}
                    }
                }
                catch (Exception ex)
                {
                }
                ////else
                ////{
                ////    strstatus = GetStatus(dateoffice, dateIN1, dateOUT2, empid);
                ////}

                //if (totalworkafterworking_status == "Present(COMPOFF)")
                //{
                //    totalpresentshowCompOff = totalpresentshowCompOff + 1;
                //    totalpresentCompOff = totalpresentCompOff + 1;
                //}
                //else if (totalworkafterworking_status == "HalfDay(COMPOFF)")
                //{
                //    totalHalfDayCompOff = totalHalfDayCompOff + 1;
                //    totalpresentshowCompOff = totalpresentshowCompOff + Convert.ToDecimal(.5);
                //}


                if (strstatus == "LeaveApproved")
                {
                    LeaveApproved = LeaveApproved + 1;
                    totalAbsent = totalAbsent + 1;
                }

                else if (strstatus == "Absent" || strstatus == "MisPunch")
                {

                    totalAbsent = totalAbsent + 1;
                    LeaveunApproved = LeaveunApproved + 1;
                    //LeaveunApproved = LeaveunApproved + 2;

                }
                //else if (strstatus == "Present" || strstatus == "Present(COMPOFF)")
                else if (strstatus == "Present")
                {
                    //else if (strstatus == "Present" || strstatus == "PresentWO" || strstatus == "MisPunch")
                    //{
                    totalpresentshow = totalpresentshow + 1;
                    totalpresent = totalpresent + 1;
                }

                else if (strstatus == "PresentLate")
                {
                    totalpresentshow = totalpresentshow + 1;
                    totallate = totallate + 1;
                    totalatecommingshow = totalatecommingshow + 1;

                }
                else if (strstatus == "Holiday")
                {
                    totalHoliday = totalHoliday + 1;
                }

                //else if (strstatus == "HalfDay" || strstatus == "HalfDay(COMPOFF)")
                else if (strstatus == "HalfDay")
                {
                    totalHalfDay = totalHalfDay + 1;
                    totalpresentshow = totalpresentshow + Convert.ToDecimal(.5);
                    //totalHalfDayapprove = totalHalfDayapprove + Convert.ToDecimal(.5);
                }

                else if (strstatus == "HalfDayLeaveApproved")
                {
                    totalpresentshow = totalpresentshow + Convert.ToDecimal(.5);
                    totalHalfDayapprove = totalHalfDayapprove + Convert.ToDecimal(.5);
                    LeaveApproved = LeaveApproved + Convert.ToDecimal(0.5);
                }
                else if (strstatus == "HalfDayLeaveunApproved")
                {
                    totalHalfDayunapprove = totalHalfDayunapprove + 1;
                }

                else if (strstatus == "LeaveunApproved")
                {
                    //LeaveunApproved = LeaveunApproved + 1;
                    LeaveunApproved = LeaveunApproved + 2;
                }
                else if (strstatus == "WeekOff")
                {
                    int frvw = 0;
                    int nexw = 0;
                    int twnexw = 0;
                    if (idate > 0 && idate < dsdate.Tables[0].Rows.Count - 2)
                    {

                        if (!showweekoff(Convert.ToDateTime(dsdate.Tables[0].Rows[idate - 1]["date"]), empid))
                        {
                            SqlParameter[] prm1111 = new SqlParameter[4];
                            prm1111[0] = new SqlParameter("@fromDate", dsdate.Tables[0].Rows[idate - 1]["date"]);
                            prm1111[1] = new SqlParameter("@toDate", dsdate.Tables[0].Rows[idate - 1]["date"]);
                            prm1111[2] = new SqlParameter("@empCode", empcod);
                            prm1111[3] = new SqlParameter("@cid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                            DataSet ds1111 = dahr.GetDataSet("AdminSearchPunchReport", prm1111);
                            if (ds1111.Tables[0].Rows.Count == 0)
                            {
                                frvw = 1;
                            }
                            if (showweekoff(Convert.ToDateTime(dsdate.Tables[0].Rows[idate + 1]["date"]), empid))
                            {
                                // twnexw = 1;
                                SqlParameter[] prm11112 = new SqlParameter[4];
                                prm11112[0] = new SqlParameter("@fromDate", dsdate.Tables[0].Rows[idate + 2]["date"]);
                                prm11112[1] = new SqlParameter("@toDate", dsdate.Tables[0].Rows[idate + 2]["date"]);
                                prm11112[2] = new SqlParameter("@empCode", empcod);
                                prm11112[3] = new SqlParameter("@cid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                                DataSet ds11112 = dahr.GetDataSet("AdminSearchPunchReport", prm11112);
                                if (ds11112.Tables[0].Rows.Count == 0)
                                {
                                    nexw = 1;
                                }

                            }
                            else
                            {
                                SqlParameter[] prm11113 = new SqlParameter[4];
                                prm11113[0] = new SqlParameter("@fromDate", dsdate.Tables[0].Rows[idate + 1]["date"]);
                                prm11113[1] = new SqlParameter("@toDate", dsdate.Tables[0].Rows[idate + 1]["date"]);
                                prm11113[2] = new SqlParameter("@empCode", empcod);
                                prm11113[3] = new SqlParameter("@cid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                                DataSet ds11113 = dahr.GetDataSet("AdminSearchPunchReport", prm11113);
                                if (ds11113.Tables[0].Rows.Count == 0)
                                {
                                    nexw = 1;
                                }
                            }
                        }
                        else
                        {




                            SqlParameter[] prm1111 = new SqlParameter[4];
                            prm1111[0] = new SqlParameter("@fromDate", dsdate.Tables[0].Rows[idate - 2]["date"]);
                            prm1111[1] = new SqlParameter("@toDate", dsdate.Tables[0].Rows[idate - 2]["date"]);
                            prm1111[2] = new SqlParameter("@empCode", empcod);
                            prm1111[3] = new SqlParameter("@cid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                            DataSet ds1111 = dahr.GetDataSet("AdminSearchPunchReport", prm1111);
                            if (ds1111.Tables[0].Rows.Count == 0)
                            {
                                frvw = 1;
                            }
                            if (showweekoff(Convert.ToDateTime(dsdate.Tables[0].Rows[idate + 1]["date"]), empid))
                            {
                                // twnexw = 1;
                                SqlParameter[] prm11112 = new SqlParameter[4];
                                prm11112[0] = new SqlParameter("@fromDate", dsdate.Tables[0].Rows[idate + 2]["date"]);
                                prm11112[1] = new SqlParameter("@toDate", dsdate.Tables[0].Rows[idate + 2]["date"]);
                                prm11112[2] = new SqlParameter("@empCode", empcod);
                                prm11112[3] = new SqlParameter("@cid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                                DataSet ds11112 = dahr.GetDataSet("AdminSearchPunchReport", prm11112);
                                if (ds11112.Tables[0].Rows.Count == 0)
                                {
                                    nexw = 1;
                                }

                            }
                            else
                            {
                                SqlParameter[] prm11113 = new SqlParameter[4];
                                prm11113[0] = new SqlParameter("@fromDate", dsdate.Tables[0].Rows[idate + 1]["date"]);
                                prm11113[1] = new SqlParameter("@toDate", dsdate.Tables[0].Rows[idate + 1]["date"]);
                                prm11113[2] = new SqlParameter("@empCode", empcod);
                                prm11113[3] = new SqlParameter("@cid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
                                DataSet ds11113 = dahr.GetDataSet("AdminSearchPunchReport", prm11113);
                                if (ds11113.Tables[0].Rows.Count == 0)
                                {
                                    nexw = 1;
                                }
                            }








                        }



                    }

                    if (frvw == 1 && nexw == 1)
                    {
                        totalAbsent = totalAbsent + 1;
                        //LeaveunApproved = LeaveunApproved + 1;
                        LeaveunApproved = LeaveunApproved + 2;

                    }
                    else
                    {
                        totalWeekOff = totalWeekOff + 1;

                    }
                }
                else if (strstatus == "Present(WO)")
                {
                    totalWeekOff = totalWeekOff + 1;
                }
                else if (strstatus == "halfODApproved")
                {
                    halfodApproved = halfodApproved + 1;
                }
                else if (strstatus == "halfODunApproved")
                {
                    halfodunApproved = halfodunApproved + 1;
                }
                else if (strstatus == "ODApproved")
                {
                    odApproved = odApproved + 1;
                    totalAbsent = totalAbsent + 1;
                }
                else if (strstatus == "ODunApproved")
                {
                    odunApproved = odunApproved + 1;
                    totalAbsent = totalAbsent + 1;
                }


                //}

                //if (totallate > 3)
                //{

                //}

                //totalpaid=

                // }
                // retstr = totalpaid.ToString() ;

                //retstr = totalpaid.ToString() + " totallate " + totallate + " totalpresent " + totalpresent + " totalHoliday " + totalHoliday + " totalHalfDay " + totalHalfDay + " totalAbsent " + totalAbsent;



            }
        }

        decimal had = 2;
        totalod = odApproved + (halfodApproved / had);

        totalunod = odunApproved + (halfodunApproved / had);

        // totalAbsent = totalAbsent + totalHalfDay / had;

        totallate = totallate / 4;

        totalHalfDay = totalHalfDay + totallate;
        totalHalfDayshow = totalHalfDay;
        totalHalfDay = totalHalfDay / had;




        decimal totalunapproveday = LeaveunApproved;
        decimal totalhalfunapproveday = totalHalfDayunapprove;


        //decimal totalunapproveday = LeaveunApproved * 2;
        //decimal totalhalfunapproveday = totalHalfDayunapprove * 2;


        decimal totalHalfDayunapproveday = totalhalfunapproveday / had;
        // totalpaid = totalpresent + totalHoliday + totalWeekOff - totalHalfDay - totalAbsent;

        decimal leaveadj = gettotalleave(empid);
        //decimal totalab = totalHalfDay + totalunapproveday + totalHalfDayunapproveday + LeaveApproved + totalunod;
        decimal totalab = totalHalfDay + totalunapproveday + totalHalfDayunapproveday + totalunod ;


        //  strssss = totalHalfDay + "+ " + totalunapproveday + "+ " + totalHalfDayunapproveday + "+ " + LeaveApproved + "+ " + totalunod;

        if (leaveadj > 3)
        {
            leaveadj = 3;
        }

        //LeaveApproved = LeaveApproved + totalHalfDayapprove;


        if (leaveadj >= LeaveApproved)
        {
            totalpaid = Convert.ToDecimal(days) - totalab + LeaveApproved;// +totalpresentshow;
            totalleaveadc = LeaveApproved;// +totalHalfDayapprove;
        }
        else
        {
            decimal taldiff = LeaveApproved;// +totalHalfDayapprove;
            if (leaveadj > 0)
            {
                taldiff = LeaveApproved - leaveadj;
                totalleaveadc = leaveadj;
            }
            totalpaid = Convert.ToDecimal(days) - totalab + totalleaveadc;// +totalpresentshow;
           // totalpaid = Convert.ToDecimal(days) - totalab - taldiff;// +totalpresentshow;
        }
        if (totalpaid < 0)
        {
            totalpaid = 0;
        }

        totalpaid = totalpaid + totalpresentshowCompOff;

        //totalpresentshow = totalpresentshow + totalpresentCompOff;

        //string strtable = totalpresentshow + "###" + totalHoliday + "###" + totalWeekOff + "###" + totalHalfDayshow + "###" + totalAbsent + "###" + LeaveApproved + "###" + LeaveunApprovedshow + "###" + totalleaveadc + "###" + totalatecommingshow + "###" + totalod + "###" + totalunod;

        string strtable = totalpresentshow + "###" + totalHoliday + "###" + totalWeekOff + "###" + totalHalfDayshow + "###" + totalAbsent + "###" + LeaveApproved + "###" + LeaveunApprovedshow + "###" + totalleaveadc + "###" + totalatecommingshow + "###" + totalod + "###" + totalunod + "###" + totalpresentCompOff + "###" + totalHalfDayCompOff;


        //retstr = totalpaid.ToString() + "<br/><table class='table table-striped table-bordered table-hover'><tr><td>Payable Day</td><td>Present</td><td>Holiday</td><td>Week Off</td><td>HalfDay</td><td>Absent</td><td>Leave Approved</td><td>Leave Unapproved</td><td>Adjust Leave</td><td>Approved OD</td><td>Unapproved OD</td</tr>";

        retstr = totalpaid.ToString() + "<br/><table class='table table-striped table-bordered table-hover'><tr><td>Payable Day</td><td>Present</td><td>Holiday</td><td>Week Off</td><td>HalfDay</td><td>Absent</td><td>Leave Approved</td><td>Leave Unapproved</td><td>Adjust Leave</td><td>Approved OD</td><td>Unapproved OD</td></tr>";

       // retstr = totalpaid.ToString() + "<br/><table class='table table-striped table-bordered table-hover'><tr><td>Payable Day</td><td>Present</td><td>Holiday</td><td>Week Off</td><td>HalfDay</td><td>Absent</td><td>Leave Approved</td><td>Leave Unapproved</td><td>Adjust Leave</td><td>Approved OD</td><td>Unapproved OD</td><td>Comp OFF Full Day</td><td>Comp OFF HalfDay</td></tr>";
        
        
        
        ////retstr += "<tr><td>"+ totalpresentshow + "</td><td>" + totalHoliday + "</td><td>" + totalWeekOff + "</td><td>" + totalHalfDayshow + "</td><td>" + totalAbsent + "</td><td>" + LeaveApproved + "</td><td>" + LeaveunApprovedshow + "</td><td>" + totalleaveadc + "</td><td>" + totalod + "</td><td>" + totalunod + "</td></tr></table><br/>";

        //retstr += "<tr><td><a href='PunchReport.aspx?empcode=" + cf.Encrypt(empcod) + "&mid=" + cf.Encrypt(Convert.ToDateTime(mdate).Month.ToString()) + "&yid=" + cf.Encrypt(Convert.ToDateTime(mdate).Year.ToString()) + "' target='_blank'>" + totalpaid.ToString() + "</a></td><td>" + totalpresentshow + "</td><td>" + totalHoliday + "</td><td>" + totalWeekOff + "</td><td>" + totalHalfDayshow + "</td><td>" + totalAbsent + "</td><td>" + LeaveApproved + "</td><td>" + LeaveunApprovedshow + "</td><td>" + totalleaveadc + "</td><td>" + totalod + "</td><td>" + totalunod + "</td></tr></table><br/>";
        retstr += "<tr><td><a href='PunchReport.aspx?empcode=" + cf.Encrypt(empcod) + "&mid=" + cf.Encrypt(Convert.ToDateTime(mdate).Month.ToString()) + "&yid=" + cf.Encrypt(Convert.ToDateTime(mdate).Year.ToString()) + "' target='_blank'>" + totalpaid.ToString() + "</a></td><td>" + totalpresentshow + "</td><td>" + totalHoliday + "</td><td>" + totalWeekOff + "</td><td>" + totalHalfDayshow + "</td><td>" + totalAbsent + "</td><td>" + LeaveApproved + "</td><td>" + LeaveunApprovedshow + "</td><td>" + totalleaveadc + "</td><td>" + totalod + "</td><td>" + totalunod + "</td></tr></table><br/>";

      //  retstr += "<tr><td><a href='PunchReport.aspx?empcode=" + cf.Encrypt(empcod) + "&mid=" + cf.Encrypt(Convert.ToDateTime(mdate).Month.ToString()) + "&yid=" + cf.Encrypt(Convert.ToDateTime(mdate).Year.ToString()) + "' target='_blank'>" + totalpaid.ToString() + "</a></td><td>" + totalpresentshow + "</td><td>" + totalHoliday + "</td><td>" + totalWeekOff + "</td><td>" + totalHalfDayshow + "</td><td>" + totalAbsent + "</td><td>" + LeaveApproved + "</td><td>" + LeaveunApprovedshow + "</td><td>" + totalleaveadc + "</td><td>" + totalod + "</td><td>" + totalunod + "</td><td>" + totalpresentCompOff + "</td><td>" + totalHalfDayCompOff + "</td></tr></table><br/>";
        
        retstr += strtable;

        // HttpContext.Current.Response.Write("<br/>end ");
        return retstr;

    }

    //public string gettotalpaidday(string mdate, string empcod, string days,string empid)
    //{
    //    dataaccesshr dahr = new dataaccesshr();
    //    string retstr = "";
       
    //    SqlParameter[] prm = new SqlParameter[3];
    //    prm[0] = new SqlParameter("@fromDate", mdate);
    //    prm[1] = new SqlParameter("@toDate", getlastdateofthemonth(mdate));
    //    prm[2] = new SqlParameter("@empCode", empcod);
    //    DataSet ds = dahr.GetDataSet("AdminSearchPunchReport", prm);



    //    decimal totalpaid = 0;
    //    decimal totalAbsent = 0;
    //    decimal totalpresent = 0;
    //    decimal totalpresentshow = 0;
    //    int totallate = 0;
    //    decimal totalHalfDay = 0;
    //    decimal totalHalfDayshow = 0;
    //    decimal totalHalfDayapprove = 0;
    //    decimal totalHalfDayunapprove = 0;
    //    decimal totalHoliday = 0;
    //    decimal totalWeekOff = 0;
    //    decimal LeaveunApproved = 0;
    //    decimal LeaveunApprovedshow = 0;
    //    decimal LeaveApproved = 0;
    //    decimal totalleaveadc = 0;
    //    decimal odApproved = 0;
    //       decimal odunApproved = 0;
    //       decimal totalod = 0;
    //       decimal totalunod = 0;
    //       decimal halfodApproved = 0;
    //       decimal halfodunApproved = 0;
    //       string strssss = "";
    //    decimal totalatecommingshow = 0;
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {

    //        string strstatus = empcod + " " + mdate + " " + getlastdateofthemonth(mdate)+"<br/>";
    //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //        {

    //            strstatus = GetStatus(ds.Tables[0].Rows[i]["DateOFFICE"].ToString(), ds.Tables[0].Rows[i]["IN1"].ToString(), ds.Tables[0].Rows[i]["OUT2"].ToString(), empid);
    //            if (strstatus == "LeaveApproved")
    //            {
    //                LeaveApproved = LeaveApproved + 1;
    //                totalAbsent = totalAbsent + 1;
    //            }
                
    //            if (strstatus == "Absent" )
    //             {

    //                 totalAbsent = totalAbsent+1;
    //                 LeaveunApproved = LeaveunApproved + 1;
                   
    //             }
    //             else if (strstatus == "Present" || strstatus == "PresentWO" || strstatus=="MisPunch")
    //             {
    //                 totalpresentshow = totalpresentshow + 1;
    //                 totalpresent = totalpresent + 1;
    //             }
    //             else if (strstatus == "PresentLate")
    //             {
    //                 totalpresentshow = totalpresentshow + 1;
    //                 totallate = totallate + 1;
    //                 totalatecommingshow = totalatecommingshow + 1;

    //             }
    //             else if (strstatus == "Holiday")
    //             {
    //                 totalHoliday = totalHoliday + 1;
    //             }
    //             else if (strstatus == "HalfDay" || strstatus == "HalfDayLeaveApproved")
    //             {
    //                 totalHalfDay = totalHalfDay + 1;
    //                 totalpresentshow = totalpresentshow + Convert.ToDecimal(.5);

    //             }
                
    //             else if (strstatus == "HalfDayLeaveunApproved")
    //             {
    //                 totalHalfDayunapprove = totalHalfDayunapprove + 1;
    //             }
                 
    //             else if (strstatus == "LeaveunApproved")
    //             {
    //                 LeaveunApproved = LeaveunApproved + 1;
    //             }
    //             else if (strstatus == "WeekOff")
    //             {
    //                 totalWeekOff = totalWeekOff + 1;
    //             }
    //            else if (strstatus == "halfODApproved")
    //            {
    //                halfodApproved = halfodApproved + 1;
    //            }
    //            else if (strstatus == "halfODunApproved")
    //            {
    //                halfodunApproved = halfodunApproved + 1;
    //            }
    //            else if (strstatus == "ODApproved")
    //            {
    //                odApproved = odApproved + 1;
    //                totalAbsent = totalAbsent + 1;
    //            }
    //            else if (strstatus == "ODunApproved")
    //            {
    //                odunApproved = odunApproved + 1;
    //                totalAbsent = totalAbsent + 1;
    //            }
          
            
    //        }
    //          decimal had = 2;
    //          totalod = odApproved + (halfodApproved / had);

    //          totalunod = odunApproved + (halfodunApproved / had);

    //        totallate = totallate / 4;
    //        totalHalfDay=totalHalfDay+totallate;
    //        totalHalfDayshow = totalHalfDay;
          
    //        totalHalfDay = totalHalfDay / had;


    //        decimal totalunapproveday = LeaveunApproved;
    //        decimal totalhalfunapproveday = totalHalfDayunapprove;

    //        //decimal totalunapproveday = LeaveunApproved * 2;
    //        //decimal totalhalfunapproveday = totalHalfDayunapprove * 2;


    //        decimal totalHalfDayunapproveday = totalhalfunapproveday / had;
    //       // totalpaid = totalpresent + totalHoliday + totalWeekOff - totalHalfDay - totalAbsent;

    //        decimal leaveadj = gettotalleave(empid);
    //        decimal totalab = totalHalfDay + totalunapproveday + totalHalfDayunapproveday + LeaveApproved + totalunod;


    //       //  strssss = totalHalfDay + "+ " + totalunapproveday + "+ " + totalHalfDayunapproveday + "+ " + LeaveApproved + "+ " + totalunod;
    //        if (leaveadj >= totalab)
    //        {
    //            totalpaid = Convert.ToDecimal(days);
    //            totalleaveadc = totalab;
    //        }
    //        else
    //        {
    //            decimal taldiff = totalab;
    //            if (leaveadj > 0)
    //            {
    //                taldiff = totalab - leaveadj;
    //                totalleaveadc = leaveadj;
    //            }
    //            totalpaid = Convert.ToDecimal(days) - taldiff;
    //        }
    //        //if (totallate > 3)
    //        //{

    //        //}

    //        //totalpaid=

    //    }
    //   // retstr = totalpaid.ToString() ;

    //    //retstr = totalpaid.ToString() + " totallate " + totallate + " totalpresent " + totalpresent + " totalHoliday " + totalHoliday + " totalHalfDay " + totalHalfDay + " totalAbsent " + totalAbsent;

    //    if (totalpaid < 0)
    //    {
    //        totalpaid = 0;
    //    }
       
    //    string strtable = totalpresentshow + "###" + totalHoliday + "###" + totalWeekOff + "###" + totalHalfDayshow + "###" + totalAbsent + "###" + LeaveApproved + "###" + LeaveunApprovedshow + "###" + totalleaveadc + "###" + totalatecommingshow + "###" + totalod + "###" + totalunod;

    //    retstr = totalpaid.ToString() + "<br/><table class='table table-striped table-bordered table-hover'><tr><td>Payable Day</td><td>Present</td><td>Holiday</td><td>Week Off</td><td>HalfDay</td><td>Absent</td><td>Leave Approved</td><td>Leave Unapproved</td><td>Adjust Leave</td><td>Approved OD</td><td>Unapproved OD</td></tr>";
    //  //  retstr += "<tr><td>"+ totalpresentshow + "</td><td>" + totalHoliday + "</td><td>" + totalWeekOff + "</td><td>" + totalHalfDayshow + "</td><td>" + totalAbsent + "</td><td>" + LeaveApproved + "</td><td>" + LeaveunApprovedshow + "</td><td>" + totalleaveadc + "</td><td>" + totalod + "</td><td>" + totalunod + "</td></tr></table><br/>";

    //    retstr += "<tr><td><a href='PunchReport.aspx?empcode=" + cf.Encrypt(empcod) + "&mid=" + cf.Encrypt(Convert.ToDateTime(mdate).Month.ToString()) + "&yid=" + cf.Encrypt(Convert.ToDateTime(mdate).Year.ToString()) + "' target='_blank'>" + totalpaid.ToString() + "</a></td><td>" + totalpresentshow + "</td><td>" + totalHoliday + "</td><td>" + totalWeekOff + "</td><td>" + totalHalfDayshow + "</td><td>" + totalAbsent + "</td><td>" + LeaveApproved + "</td><td>" + LeaveunApprovedshow + "</td><td>" + totalleaveadc + "</td><td>" + totalod + "</td><td>" + totalunod + "</td></tr></table><br/>";
       
    //    retstr += strtable;
    //    return retstr;

    //}


    public decimal gettotalleave(string empid)
    {
        decimal strretn = 0;

        string strsql = "select * from sm_emp_leave_half where uid='" + empid + "'";
        DataSet dsrml = da.GetDataSetInline(strsql);
        if (dsrml.Tables[0].Rows.Count > 0)
        {
            strretn = Convert.ToDecimal(dsrml.Tables[0].Rows[0]["left_leave"].ToString());
        }

        return strretn;
    }

    public int GetWeekNumber(DateTime dtime)
    {
        //    CultureInfo ciCurr = CultureInfo.CurrentCulture;
        //    int weekNum = ciCurr.Calendar.GetWeekOfYear(dtPassed, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        //    return weekNum;
        //}
        int weekno = 0;
        SqlParameter[] prmw = new SqlParameter[1];
        prmw[0] = new SqlParameter("@DATE", dtime);

        DataSet dsw = da.GetDataSet("selectweeknofromdate", prmw);
        if (dsw.Tables[0].Rows.Count > 0)
        {
            weekno = Convert.ToInt32(dsw.Tables[0].Rows[0]["WEEK_OF_MONTH"].ToString());
        }
        return weekno;
        //int count = 0;
        //if (dtime.Day <= 7)
        //{
        //    count = 1;
        //}
        //else if (dtime.Day > 7 && dtime.Day <= 14)
        //{
        //    count = 2;
        //}
        //else if (dtime.Day > 14 && dtime.Day <= 21)
        //{
        //    count = 3;
        //}
        //else if (dtime.Day > 21 && dtime.Day <= 28)
        //{
        //    count = 4;
        //}
        //else if (dtime.Day > 28)
        //{
        //    count = 5;
        //}

        //return count;
    }

    public int GetdayNumber(DateTime dtime)
    {
        //    CultureInfo ciCurr = CultureInfo.CurrentCulture;
        //    int weekNum = ciCurr.Calendar.GetWeekOfYear(dtPassed, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        //    return weekNum;
        //}

        int count = 0;
        int day = (int)dtime.DayOfWeek;
        count = day + 1;
        return count;
    }


    public bool showweekoff(DateTime dtime, string empid)
    {

        int wcount = 0;
        int dno = 0;
        wcount = GetWeekNumber(Convert.ToDateTime(dtime));
        dno = GetdayNumber(Convert.ToDateTime(dtime));
        // Response.Write("hi" + wcount + " " + dno);
        //  Response.Write(dno);
        string strdays = "0";
        string strsqlemptime = "select * from tbl_emptiming where id in(select ddlshift from tbl_employee where  id='" + empid + "') and status=1 and archive=0";
      //  string strsqlemptime = "select * from tbl_emptiming where empid='" + empid + "' and status=1 and archive=0";
        DataSet dstimeemp = da.GetDataSetInline(strsqlemptime);

        if (dstimeemp.Tables[0].Rows.Count > 0)
        {
            switch (wcount)
            {
                case 1:
                    strdays = dstimeemp.Tables[0].Rows[0]["weeklyoff"].ToString();
                    break;
                case 2:
                    strdays = dstimeemp.Tables[0].Rows[0]["weeklyoff2"].ToString();
                    break;
                case 3:
                    strdays = dstimeemp.Tables[0].Rows[0]["weeklyoff3"].ToString();
                    break;
                case 4:
                    strdays = dstimeemp.Tables[0].Rows[0]["weeklyoff4"].ToString();
                    break;
                case 5:
                    strdays = dstimeemp.Tables[0].Rows[0]["weeklyoff5"].ToString();
                    break;

            }

            string strsql = "select * from dbo.UF_CSVToTable('" + strdays + "') where sid=" + dno + "";
            //  Response.Write(strsql+"<br/>");
            DataSet dstrue = da.GetDataSetInline(strsql);
            if (dstrue.Tables[0].Rows.Count > 0)
            {
                return true;
            }

        }


        return false;

    }

    string GetStatus(string date, string sDate, string eDate, string empid)
    {
        int isonleave = getleavedetails(Convert.ToDateTime(date), empid);

        DataAccess daaa = new DataAccess();
        string retStr = "";
        if (sDate != string.Empty && eDate != string.Empty)
        {
            retStr = "Present";

            string[] da = date.Split(' ');
            string csDate = da[0] + " 12:00:00 PM";
            string ceDate = da[0] + " 4:00:00 PM";

            string lsDate = da[0] + " 9:33:00 AM";
            string leDate = da[0] + " 6:00:00 PM";


            //  string strsql="select * from 


            string stremptime = "select * from tbl_emptiming where id in(select ddlshift from tbl_employee where  id='" + empid + "') and status=1 and archive=0";

            DataSet empdatasettime = daaa.GetDataSetInline(stremptime);

            if (empdatasettime.Tables[0].Rows.Count > 0)
            {

                try
                {
                    if (empdatasettime.Tables[0].Rows[0]["InTime"].ToString() != string.Empty)
                    {
                        lsDate = Convert.ToDateTime(da[0] + " " + empdatasettime.Tables[0].Rows[0]["InTime"].ToString()).AddMinutes(Convert.ToInt32(empdatasettime.Tables[0].Rows[0]["RedMark"].ToString())).ToString();
                    }
                }
                catch (Exception)
                {

                }
                try
                {
                    if (empdatasettime.Tables[0].Rows[0]["outtime"].ToString() != string.Empty)
                    {
                        leDate = Convert.ToDateTime(da[0] + " " + empdatasettime.Tables[0].Rows[0]["outtime"].ToString()).ToString();
                    }
                }
                catch (Exception)
                {

                }
                try
                {
                    if (empdatasettime.Tables[0].Rows[0]["halfday"].ToString() != string.Empty)
                    {
                        csDate = Convert.ToDateTime(da[0] + " " + empdatasettime.Tables[0].Rows[0]["InTime"].ToString()).AddMinutes(Convert.ToInt32(empdatasettime.Tables[0].Rows[0]["halfday"].ToString())).ToString();
                    }

                }
                catch (Exception)
                {

                }
                try
                {
                    if (empdatasettime.Tables[0].Rows[0]["halfday"].ToString() != string.Empty)
                    {
                        ceDate = Convert.ToDateTime(da[0] + " " + empdatasettime.Tables[0].Rows[0]["outtime"].ToString()).AddHours(-Convert.ToInt32(empdatasettime.Tables[0].Rows[0]["halfday"].ToString())).ToString();
                    }
                }
                catch (Exception)
                {

                }
                //csDate = da[0] + " 12:00:00 PM";
                //ceDate = da[0] + " 4:00:00 PM";

                //lsDate = da[0] + " 9:33:00 AM";
                //leDate = da[0] + " 6:00:00 PM";

            }

            //DataAccess da1 = new DataAccess();
            //SqlParameter[] sqlparam = new SqlParameter[2];
            //sqlparam[0] = new SqlParameter("@date", date);
            //sqlparam[1] = new SqlParameter("@uid", empid);
            ////DataSet ds = da.GetDataSet("[dbo].[Hariyali_SelectHoliday]", sqlparam);

            //DataSet dsHoliday = da1.GetDataSet("[dbo].[Hariyali_SelectHoliday_new]", sqlparam);
            //if (Convert.ToInt32(dsHoliday.Tables[0].Rows[0]["RCount"]) > 0)
            //{
            //    retStr = "Present(Holiday)";
            //}
            //else 

            if (showweekoff(Convert.ToDateTime(date), empid))
            {
                retStr = "Present(WO)";
            }
            else
            {
                // if (Convert.ToDateTime(sDate) > Convert.ToDateTime(lsDate) || Convert.ToDateTime(eDate) < Convert.ToDateTime(leDate))
                if (Convert.ToDateTime(sDate) > Convert.ToDateTime(lsDate))
                {
                    retStr = "PresentLate";
                }
                //  if (Convert.ToDateTime(sDate) > Convert.ToDateTime(csDate) || Convert.ToDateTime(eDate) < Convert.ToDateTime(ceDate))
                if (Convert.ToDateTime(sDate) > Convert.ToDateTime(csDate))
                {
                    retStr = "HalfDay";

                    if (isonleave == 1)
                    {
                        retStr = "HalfDayLeaveApproved";


                        ////DataAccess daaa = new DataAccess();
                        //string str111 = "select * from sm_emp_leaves_details_half where sm_emp_leaves_details_half.status=1 ";
                        //str111 += " and sm_emp_leaves_details_half.archive=0 and sm_emp_leaves_details_half.uid='" + empid + "' and ";
                        //str111 += "  '" + date + "' between sm_emp_leaves_details_half.Fromdate   and sm_emp_leaves_details_half.Todate";
                        //DataSet dsleave111 = daaa.GetDataSetInline(str111);
                        //if (dsleave111.Tables[0].Rows.Count > 0)
                        //{
                        //    if (Convert.ToDateTime(dsleave111.Tables[0].Rows[0]["Fromdate"]) == Convert.ToDateTime(dsleave111.Tables[0].Rows[0]["Todate"]))
                        //    {
                        //        if (dsleave111.Tables[0].Rows[0]["no_of_day"].ToString() == "0.5")
                        //        {
                        //            if (dsleave111.Tables[0].Rows[0]["admin_access"].ToString() == "2")
                        //            {
                        //                retStr = "HalfDayLeaveApproved";
                        //            }
                        //            else
                        //            {
                        //                //if (dsleave111.Tables[0].Rows[0]["isapproved"].ToString() == "3")
                        //                //{
                        //                retStr = "HalfDayLeaveunApproved";
                        //                //}
                        //                //else
                        //                //{
                        //                //    retStr = "Half Day Leave Applied(In Process)";
                        //                //}
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (dsleave111.Tables[0].Rows[0]["admin_access"].ToString() == "2")
                        //        {
                        //            retStr = "HalfDayLeaveApproved";
                        //        }
                        //        else
                        //        {
                        //            retStr = "HalfDayLeaveunApproved";
                        //        }

                        //    }

                        //}
                    }
                    else
                    {

                        string strod = "select * from tbl_ondutydetails where tbl_ondutydetails.status=1 ";
                        strod += " and tbl_ondutydetails.archive=0 and tbl_ondutydetails.uid='" + empid + "' and ";
                        strod += "  CAST ('" + date + "' as date) between CAST (tbl_ondutydetails.Fromdate as date)   and CAST (tbl_ondutydetails.Todate as date) and ishrapprove=2";
                        DataSet dsod = daaa.GetDataSetInline(strod);
                        // Response.Write(strod+"<br/>");

                        if (dsod.Tables[0].Rows.Count > 0)
                        {

                            if (dsod.Tables[0].Rows[0]["ishrapprove"].ToString() == "2")
                            {
                                retStr = "ODApproved";
                            }
                            else
                            {
                                retStr = "ODunApproved";

                            }
                        }
                        else
                        {
                            string strod1 = "select * from tbl_ondutydetails where tbl_ondutydetails.status=1 ";
                            strod1 += " and tbl_ondutydetails.archive=0 and tbl_ondutydetails.uid='" + empid + "' and ";
                            strod1 += "   CAST ('" + date + "' as date) between CAST (tbl_ondutydetails.Fromdate as date)   and CAST (tbl_ondutydetails.Todate as date) ";
                            DataSet dsod1 = daaa.GetDataSetInline(strod1);
                            if (dsod1.Tables[0].Rows.Count > 0)
                            {

                                if (dsod1.Tables[0].Rows[0]["ishrapprove"].ToString() == "2")
                                {
                                    retStr = "ODApproved";
                                }
                                else
                                {
                                    retStr = "ODunApproved";

                                }
                            }
                            else
                            {
                                // retStr += "<br/><a href=Leaveform.aspx>Apply For Half Day Leave</a>";
                            }
                        }





                    }


                }


                if (isonleave == 1)
                {

                    retStr = "HalfDayLeaveApproved";

                    //string str = "select * from sm_emp_leaves_details_half where sm_emp_leaves_details_half.status=1 ";
                    //str += " and sm_emp_leaves_details_half.archive=0 and sm_emp_leaves_details_half.uid='" + empid + "' and ";
                    //str += "  '" + date + "' between sm_emp_leaves_details_half.Fromdate   and sm_emp_leaves_details_half.Todate and is_admincancel is null";
                    //DataSet dsleave = daaa.GetDataSetInline(str);
                    //if (dsleave.Tables[0].Rows.Count > 0)
                    //{
                    //    if (Convert.ToDateTime(dsleave.Tables[0].Rows[0]["Fromdate"]) == Convert.ToDateTime(dsleave.Tables[0].Rows[0]["Todate"]))
                    //    {
                    //        if (dsleave.Tables[0].Rows[0]["no_of_day"].ToString() == "0.5")
                    //        {
                    //            if (dsleave.Tables[0].Rows[0]["admin_access"].ToString() == "2")
                    //            {
                    //                retStr = "HalfDayLeaveApproved";
                    //            }
                    //            else
                    //            {
                    //                //if (dsleave.Tables[0].Rows[0]["isapproved"].ToString() == "3")
                    //                //{
                    //                retStr = "HalfDayLeaveunApproved";
                    //                //}
                    //                //else
                    //                //{
                    //                //    retStr = "Half Day Leave Applied(In Process)";
                    //                //}
                    //            }
                    //        }
                    //        else
                    //        {
                    //            retStr = "LeaveApproved";
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (dsleave.Tables[0].Rows[0]["admin_access"].ToString() == "2")
                    //        {
                    //            retStr = "HalfDayLeaveApproved";
                    //        }
                    //        else
                    //        {
                    //            retStr = "HalfDayLeaveunApproved";
                    //        }

                    //    }

                    //}
                }
                else
                {

                    string strod = "select * from tbl_ondutydetails where tbl_ondutydetails.status=1 ";
                    strod += " and tbl_ondutydetails.archive=0 and tbl_ondutydetails.uid='" + empid + "' and ";
                    strod += "  CAST ('" + date + "' as date) between CAST (tbl_ondutydetails.Fromdate as date)   and CAST (tbl_ondutydetails.Todate as date) and ishrapprove=2";
                    DataSet dsod = daaa.GetDataSetInline(strod);
                    // Response.Write(strod+"<br/>");

                    if (dsod.Tables[0].Rows.Count > 0)
                    {

                        if (dsod.Tables[0].Rows[0]["ishrapprove"].ToString() == "2")
                        {
                            retStr = "ODApproved";
                        }
                        else if (dsod.Tables[0].Rows[0]["ishrapprove"].ToString() == "3")
                        {
                            retStr = "ODunApproved";

                        }
                    }
                    else
                    {
                        string strod1 = "select * from tbl_ondutydetails where tbl_ondutydetails.status=1 ";
                        strod1 += " and tbl_ondutydetails.archive=0 and tbl_ondutydetails.uid='" + empid + "' and ";
                        strod1 += "   CAST ('" + date + "' as date) between CAST (tbl_ondutydetails.Fromdate as date)   and CAST (tbl_ondutydetails.Todate as date) ";
                        DataSet dsod1 = daaa.GetDataSetInline(strod1);
                        if (dsod1.Tables[0].Rows.Count > 0)
                        {

                            if (dsod1.Tables[0].Rows[0]["ishrapprove"].ToString() == "2")
                            {
                                retStr = "ODApproved";
                            }
                            else if (dsod1.Tables[0].Rows[0]["ishrapprove"].ToString() == "3")
                            {
                                retStr = "ODunApproved";

                            }
                        }
                        else
                        {
                            //retStr += "<br/><a href=Leaveform.aspx>Apply For Half Day Leave</a>";
                        }
                    }





                }


                //}
            }
        }
        else if (sDate == string.Empty && eDate == string.Empty)
        {
            retStr = "Absent";
            SqlParameter[] sqlparam = new SqlParameter[3];
            sqlparam[0] = new SqlParameter("@date", date);
            sqlparam[1] = new SqlParameter("@uid", empid);
            sqlparam[2] = new SqlParameter("@cid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
            //DataSet ds = da.GetDataSet("[dbo].[Hariyali_SelectHoliday]", sqlparam);

            DataSet ds = da.GetDataSet("[dbo].[Hariyali_SelectHoliday_new]", sqlparam);
            if (Convert.ToInt32(ds.Tables[0].Rows[0]["RCount"]) > 0)
                retStr = "Holiday";
            else if (showweekoff(Convert.ToDateTime(date), empid))
            {
                retStr = "WeekOff";
            }
            //else if (Convert.ToDateTime(date).DayOfWeek.ToString() == "Saturday" && Convert.ToDateTime(date).Day > 7 && Convert.ToDateTime(date).Day < 15)
            //{
            //    retStr = "Week Off";

            //}
            else
            {
                //string str = "select * from sm_emp_leaves_details_half where sm_emp_leaves_details_half.status=1 ";
                //str += " and sm_emp_leaves_details_half.archive=0 and sm_emp_leaves_details_half.uid='" + empid + "' and ";
                //str += "  '" + date + "' between sm_emp_leaves_details_half.Fromdate   and sm_emp_leaves_details_half.Todate and is_admincancel is null";
                //DataSet dsleave = da.GetDataSetInline(str);
                //if (dsleave.Tables[0].Rows.Count > 0)
                //{
                //    if (Convert.ToDateTime(dsleave.Tables[0].Rows[0]["Fromdate"]) == Convert.ToDateTime(dsleave.Tables[0].Rows[0]["Todate"]))
                //    {
                //        if (dsleave.Tables[0].Rows[0]["no_of_day"].ToString() == "1")
                //        {
                //            if (dsleave.Tables[0].Rows[0]["admin_access"].ToString() == "2")
                //            {
                //                retStr = "LeaveApproved";
                //            }
                //            else
                //            {
                //                if (dsleave.Tables[0].Rows[0]["isapproved"].ToString() == "3")
                //                {
                //                    retStr = "LeaveunApproved";
                //                }
                //                else
                //                {
                //                    retStr = "LeaveunApproved";
                //                }
                //            }
                //        }
                //        else if (dsleave.Tables[0].Rows[0]["no_of_day"].ToString() == "0.5")
                //        {
                //            if (dsleave.Tables[0].Rows[0]["admin_access"].ToString() == "2")
                //            {
                //                retStr = "HalfDayLeaveApproved";
                //            }
                //            else
                //            {
                //                if (dsleave.Tables[0].Rows[0]["isapproved"].ToString() == "3")
                //                {
                //                    retStr = "HalfDayLeaveunApproved";
                //                }
                //                else
                //                {
                //                    retStr = "HalfDayLeaveunApproved";
                //                }
                //            }

                //            // retStr += "<br/><a href=Leaveform.aspx>Apply For Half Day Leave</a>";
                //        }
                //    }
                //    else
                //    {

                //        if (dsleave.Tables[0].Rows[0]["admin_access"].ToString() == "2")
                //        {
                //            retStr = "LeaveApproved";
                //        }
                //        else
                //        {
                //            if (dsleave.Tables[0].Rows[0]["isapproved"].ToString() == "3")
                //            {
                //                retStr = "LeaveunApproved";
                //            }
                //            else
                //            {
                //                retStr = "LeaveunApproved";
                //            }
                //        }


                //    }
                //}


                if (isonleave == 1)
                {
                    retStr = "LeaveApproved";
                }


                string strod = "select * from tbl_ondutydetails where tbl_ondutydetails.status=1 ";
                strod += " and tbl_ondutydetails.archive=0 and tbl_ondutydetails.uid='" + empid + "' and ";
                strod += "  CAST ('" + date + "' as date) between CAST (tbl_ondutydetails.Fromdate as date)   and CAST (tbl_ondutydetails.Todate as date) and ishrapprove=2";



                DataSet dsod = da.GetDataSetInline(strod);
                if (dsod.Tables[0].Rows.Count > 0)
                {

                    if (dsod.Tables[0].Rows[0]["ishrapprove"].ToString() == "2")
                    {
                        retStr = "ODApproved";
                    }
                    else if (dsod.Tables[0].Rows[0]["ishrapprove"].ToString() == "3")
                    {
                        retStr = "ODunApproved";

                    }
                }
                else
                {
                    string strod1 = "select * from tbl_ondutydetails where tbl_ondutydetails.status=1 ";
                    strod1 += " and tbl_ondutydetails.archive=0 and tbl_ondutydetails.uid='" + empid + "' and ";
                    strod1 += "   CAST ('" + date + "' as date) between CAST (tbl_ondutydetails.Fromdate as date)   and CAST (tbl_ondutydetails.Todate as date) ";
                    DataSet dsod1 = da.GetDataSetInline(strod1);
                    if (dsod1.Tables[0].Rows.Count > 0)
                    {

                        if (dsod1.Tables[0].Rows[0]["ishrapprove"].ToString() == "2")
                        {
                            retStr = "ODApproved";
                        }
                        else if (dsod1.Tables[0].Rows[0]["ishrapprove"].ToString() == "3")
                        {
                            retStr = "ODunApproved";

                        }
                    }
                }

            }
        }
        else
        {
            //SqlParameter[] sqlparam = new SqlParameter[2];
            //sqlparam[0] = new SqlParameter("@date", date);
            //sqlparam[1] = new SqlParameter("@uid", empid);
            ////DataSet ds = da.GetDataSet("[dbo].[Hariyali_SelectHoliday]", sqlparam);

            //DataSet ds = da.GetDataSet("[dbo].[Hariyali_SelectHoliday_new]", sqlparam);
            //if (Convert.ToInt32(ds.Tables[0].Rows[0]["RCount"]) > 0)
            //    retStr = "Holiday";
            //else if (showweekoff(Convert.ToDateTime(date), empid))
            //{
            //    retStr = "WeekOff";
            //}

            if (Convert.ToDateTime(date).DayOfWeek.ToString() == "Sunday")
            {
                retStr = "MisPunch";
            }
            else
            {
                //DataAccess daaa = new DataAccess();

                string strod = "select * from tbl_ondutydetails where tbl_ondutydetails.status=1 ";
                strod += " and tbl_ondutydetails.archive=0 and tbl_ondutydetails.uid='" + empid + "' and ";
                strod += "  CAST ('" + date + "' as date) between CAST (tbl_ondutydetails.Fromdate as date)   and CAST (tbl_ondutydetails.Todate as date) and ishrapprove=2";
                DataSet dsod = daaa.GetDataSetInline(strod);
                if (dsod.Tables[0].Rows.Count > 0)
                {

                    if (dsod.Tables[0].Rows[0]["ishrapprove"].ToString() == "2")
                    {
                        retStr = "ODApproved";
                    }
                    else if (dsod.Tables[0].Rows[0]["ishrapprove"].ToString() == "3")
                    {
                        retStr = "ODunApproved";

                    }
                }
                else
                {

                    string strod1 = "select * from tbl_ondutydetails where tbl_ondutydetails.status=1 ";
                    strod1 += " and tbl_ondutydetails.archive=0 and tbl_ondutydetails.uid='" + empid + "' and ";
                    strod1 += "   CAST ('" + date + "' as date) between CAST (tbl_ondutydetails.Fromdate as date)   and CAST (tbl_ondutydetails.Todate as date) ";
                    DataSet dsod1 = daaa.GetDataSetInline(strod1);
                    if (dsod1.Tables[0].Rows.Count > 0)
                    {

                        if (dsod1.Tables[0].Rows[0]["ishrapprove"].ToString() == "2")
                        {
                            retStr = "ODApproved";
                        }
                        else if (dsod1.Tables[0].Rows[0]["ishrapprove"].ToString() == "3")
                        {
                            retStr = "ODunApproved";

                        }
                    }
                    else
                    {
                        retStr = "MisPunch";
                    }
                }




            }
        }

        return retStr;
    }


    //string GetStatus(string date, string sDate, string eDate, string empid)
    //{
    //    DataAccess daaa = new DataAccess();
    //    string retStr = "";
    //    if (sDate != string.Empty && eDate != string.Empty)
    //    {
    //        retStr = "Present";

    //        string[] da = date.Split(' ');
    //        string csDate = da[0] + " 12:00:00 PM";
    //        string ceDate = da[0] + " 4:00:00 PM";

    //        string lsDate = da[0] + " 9:33:00 AM";
    //        string leDate = da[0] + " 6:00:00 PM";


    //      //  string strsql="select * from 


    //        string stremptime = "select * from tbl_emptiming where id in(select ddlshift from tbl_employee where  id='" + empid + "') and status=1 and archive=0";

    //        DataSet empdatasettime = daaa.GetDataSetInline(stremptime);

    //        if (empdatasettime.Tables[0].Rows.Count > 0)
    //        {

    //            try
    //            {
    //                if (empdatasettime.Tables[0].Rows[0]["InTime"].ToString() != string.Empty)
    //                {
    //                    lsDate = Convert.ToDateTime(da[0] + " " + empdatasettime.Tables[0].Rows[0]["InTime"].ToString()).AddMinutes(Convert.ToInt32(empdatasettime.Tables[0].Rows[0]["RedMark"].ToString())).ToString();
    //                }
    //            }
    //            catch (Exception)
    //            {

    //            }
    //            try
    //            {
    //                if (empdatasettime.Tables[0].Rows[0]["outtime"].ToString() != string.Empty)
    //                {
    //                    leDate = Convert.ToDateTime(da[0] + " " + empdatasettime.Tables[0].Rows[0]["outtime"].ToString()).ToString();
    //                }
    //            }
    //            catch (Exception)
    //            {

    //            }
    //            try
    //            {
    //                if (empdatasettime.Tables[0].Rows[0]["halfday"].ToString() != string.Empty)
    //                {
    //                    csDate = Convert.ToDateTime(da[0] + " " + empdatasettime.Tables[0].Rows[0]["InTime"].ToString()).AddMinutes(Convert.ToInt32(empdatasettime.Tables[0].Rows[0]["halfday"].ToString())).ToString();
    //                }

    //            }
    //            catch (Exception)
    //            {

    //            }
    //            try
    //            {
    //                if (empdatasettime.Tables[0].Rows[0]["halfday"].ToString() != string.Empty)
    //                {
    //                    ceDate = Convert.ToDateTime(da[0] + " " + empdatasettime.Tables[0].Rows[0]["outtime"].ToString()).AddHours(-1).ToString();
    //                }
    //            }
    //            catch (Exception)
    //            {

    //            }
    //            //csDate = da[0] + " 12:00:00 PM";
    //            //ceDate = da[0] + " 4:00:00 PM";

    //            //lsDate = da[0] + " 9:33:00 AM";
    //            //leDate = da[0] + " 6:00:00 PM";

    //        }

    //        //DataAccess da1 = new DataAccess();
    //        //SqlParameter[] sqlparam = new SqlParameter[2];
    //        //sqlparam[0] = new SqlParameter("@date", date);
    //        //sqlparam[1] = new SqlParameter("@uid", empid);
    //        ////DataSet ds = da.GetDataSet("[dbo].[Hariyali_SelectHoliday]", sqlparam);

    //        //DataSet dsHoliday = da1.GetDataSet("[dbo].[Hariyali_SelectHoliday_new]", sqlparam);
    //        //if (Convert.ToInt32(dsHoliday.Tables[0].Rows[0]["RCount"]) > 0)
    //        //{
    //        //    retStr = "Present(Holiday)";
    //        //}
    //        //else 

    //        if (showweekoff(Convert.ToDateTime(date), empid))
    //        {
    //            retStr = "Present(WO)";
    //        }
    //        else
    //        {
    //           // if (Convert.ToDateTime(sDate) > Convert.ToDateTime(lsDate) || Convert.ToDateTime(eDate) < Convert.ToDateTime(leDate))
    //                if (Convert.ToDateTime(sDate) > Convert.ToDateTime(lsDate))
    //            {
    //                retStr = "PresentLate";
    //            }
    //          //  if (Convert.ToDateTime(sDate) > Convert.ToDateTime(csDate) || Convert.ToDateTime(eDate) < Convert.ToDateTime(ceDate))
    //                if (Convert.ToDateTime(sDate) > Convert.ToDateTime(csDate) )
    //            {
    //                retStr = "HalfDay";


    //                //DataAccess daaa = new DataAccess();
    //                string str111 = "select * from sm_emp_leaves_details_half where sm_emp_leaves_details_half.status=1 ";
    //                str111 += " and sm_emp_leaves_details_half.archive=0 and sm_emp_leaves_details_half.uid='" + empid + "' and ";
    //                str111 += "  '" + date + "' between sm_emp_leaves_details_half.Fromdate   and sm_emp_leaves_details_half.Todate";
    //                DataSet dsleave111 = daaa.GetDataSetInline(str111);
    //                if (dsleave111.Tables[0].Rows.Count > 0)
    //                {
    //                    if (Convert.ToDateTime(dsleave111.Tables[0].Rows[0]["Fromdate"]) == Convert.ToDateTime(dsleave111.Tables[0].Rows[0]["Todate"]))
    //                    {
    //                        if (dsleave111.Tables[0].Rows[0]["no_of_day"].ToString() == "0.5")
    //                        {
    //                            if (dsleave111.Tables[0].Rows[0]["admin_access"].ToString() == "2")
    //                            {
    //                                retStr = "HalfDayLeaveApproved";
    //                            }
    //                            else
    //                            {
    //                                //if (dsleave111.Tables[0].Rows[0]["isapproved"].ToString() == "3")
    //                                //{
    //                                    retStr = "HalfDayLeaveunApproved";
    //                                //}
    //                                //else
    //                                //{
    //                                //    retStr = "Half Day Leave Applied(In Process)";
    //                                //}
    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        if (dsleave111.Tables[0].Rows[0]["admin_access"].ToString() == "2")
    //                        {
    //                            retStr = "HalfDayLeaveApproved";
    //                        }
    //                        else
    //                        {
    //                            retStr = "HalfDayLeaveunApproved";
    //                        }

    //                    }

    //                }
    //                else
    //                {

    //                    string strod = "select * from tbl_ondutydetails where tbl_ondutydetails.status=1 ";
    //                    strod += " and tbl_ondutydetails.archive=0 and tbl_ondutydetails.uid='" + empid + "' and ";
    //                    strod += "  CAST ('" + date + "' as date) between CAST (tbl_ondutydetails.Fromdate as date)   and CAST (tbl_ondutydetails.Todate as date) and ishrapprove=2";
    //                    DataSet dsod = daaa.GetDataSetInline(strod);
    //                    // Response.Write(strod+"<br/>");

    //                    if (dsod.Tables[0].Rows.Count > 0)
    //                    {

    //                        if (dsod.Tables[0].Rows[0]["ishrapprove"].ToString() == "2")
    //                        {
    //                            retStr = "ODApproved";
    //                        }
    //                        else
    //                        {
    //                            retStr = "ODunApproved";

    //                        }
    //                    }
    //                    else
    //                    {
    //                        string strod1 = "select * from tbl_ondutydetails where tbl_ondutydetails.status=1 ";
    //                        strod1 += " and tbl_ondutydetails.archive=0 and tbl_ondutydetails.uid='" + empid + "' and ";
    //                        strod1 += "   CAST ('" + date + "' as date) between CAST (tbl_ondutydetails.Fromdate as date)   and CAST (tbl_ondutydetails.Todate as date) ";
    //                        DataSet dsod1 = daaa.GetDataSetInline(strod1);
    //                        if (dsod1.Tables[0].Rows.Count > 0)
    //                        {

    //                            if (dsod1.Tables[0].Rows[0]["ishrapprove"].ToString() == "2")
    //                            {
    //                                retStr = "ODApproved";
    //                            }
    //                            else
    //                            {
    //                                retStr = "ODunApproved";

    //                            }
    //                        }
    //                        else
    //                        {
    //                           // retStr += "<br/><a href=Leaveform.aspx>Apply For Half Day Leave</a>";
    //                        }
    //                    }





    //                }


    //            }




    //            //if (twh != string.Empty && twh != "--")
    //            //{
    //            //    if (Convert.ToDouble(twh) < 9)
    //            //    {
    //            //        if (Convert.ToDouble(twh) < 4.30)
    //            //        {

    //            //            retStr = "Absent";
    //            //        }
    //            //        else if (Convert.ToDouble(twh) > 8.30)
    //            //        {

    //            //            retStr = "sleavePresent";
    //            //        }
    //            //        else
    //            //        {
    //            //            retStr = "HalfDay";
    //            //        }
    //            //    }
    //            //}

    //            //DataAccess daaa = new DataAccess();
    //            string str = "select * from sm_emp_leaves_details_half where sm_emp_leaves_details_half.status=1 ";
    //            str += " and sm_emp_leaves_details_half.archive=0 and sm_emp_leaves_details_half.uid='" + empid + "' and ";
    //            str += "  '" + date + "' between sm_emp_leaves_details_half.Fromdate   and sm_emp_leaves_details_half.Todate and is_admincancel is null";
    //            DataSet dsleave = daaa.GetDataSetInline(str);
    //            if (dsleave.Tables[0].Rows.Count > 0)
    //            {
    //                if (Convert.ToDateTime(dsleave.Tables[0].Rows[0]["Fromdate"]) == Convert.ToDateTime(dsleave.Tables[0].Rows[0]["Todate"]))
    //                {
    //                    if (dsleave.Tables[0].Rows[0]["no_of_day"].ToString() == "0.5")
    //                    {
    //                        if (dsleave.Tables[0].Rows[0]["admin_access"].ToString() == "2")
    //                        {
    //                            retStr = "HalfDayLeaveApproved";
    //                        }
    //                        else
    //                        {
    //                            //if (dsleave.Tables[0].Rows[0]["isapproved"].ToString() == "3")
    //                            //{
    //                            retStr = "HalfDayLeaveunApproved";
    //                            //}
    //                            //else
    //                            //{
    //                            //    retStr = "Half Day Leave Applied(In Process)";
    //                            //}
    //                        }
    //                    }
    //                    else
    //                    {
    //                        retStr = "LeaveApproved";
    //                    }
    //                }
    //                else
    //                {
    //                    if (dsleave.Tables[0].Rows[0]["admin_access"].ToString() == "2")
    //                    {
    //                        retStr = "HalfDayLeaveApproved";
    //                    }
    //                    else
    //                    {
    //                        retStr = "HalfDayLeaveunApproved";
    //                    }

    //                }

    //            }
    //            else
    //            {

    //                string strod = "select * from tbl_ondutydetails where tbl_ondutydetails.status=1 ";
    //                strod += " and tbl_ondutydetails.archive=0 and tbl_ondutydetails.uid='" + empid + "' and ";
    //                strod += "  CAST ('" + date + "' as date) between CAST (tbl_ondutydetails.Fromdate as date)   and CAST (tbl_ondutydetails.Todate as date) and ishrapprove=2";
    //                DataSet dsod = daaa.GetDataSetInline(strod);
    //                // Response.Write(strod+"<br/>");

    //                if (dsod.Tables[0].Rows.Count > 0)
    //                {

    //                    if (dsod.Tables[0].Rows[0]["ishrapprove"].ToString() == "2")
    //                    {
    //                        retStr = "ODApproved";
    //                    }
    //                    else if (dsod.Tables[0].Rows[0]["ishrapprove"].ToString() == "3")
    //                    {
    //                        retStr = "ODunApproved";

    //                    }
    //                }
    //                else
    //                {
    //                    string strod1 = "select * from tbl_ondutydetails where tbl_ondutydetails.status=1 ";
    //                    strod1 += " and tbl_ondutydetails.archive=0 and tbl_ondutydetails.uid='" + empid + "' and ";
    //                    strod1 += "   CAST ('" + date + "' as date) between CAST (tbl_ondutydetails.Fromdate as date)   and CAST (tbl_ondutydetails.Todate as date) ";
    //                    DataSet dsod1 = daaa.GetDataSetInline(strod1);
    //                    if (dsod1.Tables[0].Rows.Count > 0)
    //                    {

    //                        if (dsod1.Tables[0].Rows[0]["ishrapprove"].ToString() == "2")
    //                        {
    //                            retStr = "ODApproved";
    //                        }
    //                        else if (dsod1.Tables[0].Rows[0]["ishrapprove"].ToString() == "3")
    //                        {
    //                            retStr = "ODunApproved";

    //                        }
    //                    }
    //                    else
    //                    {
    //                        //retStr += "<br/><a href=Leaveform.aspx>Apply For Half Day Leave</a>";
    //                    }
    //                }





    //            }


    //            //}
    //        }
    //    }
    //    else if (sDate == string.Empty && eDate == string.Empty)
    //    {
    //        retStr = "Absent";
    //        SqlParameter[] sqlparam = new SqlParameter[3];
    //        sqlparam[0] = new SqlParameter("@date", date);
    //        sqlparam[1] = new SqlParameter("@uid", empid);
    //        sqlparam[2] = new SqlParameter("@cid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
    //        //DataSet ds = da.GetDataSet("[dbo].[Hariyali_SelectHoliday]", sqlparam);

    //        DataSet ds = da.GetDataSet("[dbo].[Hariyali_SelectHoliday_new]", sqlparam);
    //        if (Convert.ToInt32(ds.Tables[0].Rows[0]["RCount"]) > 0)
    //            retStr = "Holiday";
    //        else if (showweekoff(Convert.ToDateTime(date), empid))
    //        {
    //            retStr = "WeekOff";
    //        }
    //        //else if (Convert.ToDateTime(date).DayOfWeek.ToString() == "Saturday" && Convert.ToDateTime(date).Day > 7 && Convert.ToDateTime(date).Day < 15)
    //        //{
    //        //    retStr = "Week Off";

    //        //}
    //        else
    //        {
    //            string str = "select * from sm_emp_leaves_details_half where sm_emp_leaves_details_half.status=1 ";
    //            str += " and sm_emp_leaves_details_half.archive=0 and sm_emp_leaves_details_half.uid='" + empid + "' and ";
    //            str += "  '" + date + "' between sm_emp_leaves_details_half.Fromdate   and sm_emp_leaves_details_half.Todate and is_admincancel is null";
    //            DataSet dsleave = da.GetDataSetInline(str);
    //            if (dsleave.Tables[0].Rows.Count > 0)
    //            {
    //                if (Convert.ToDateTime(dsleave.Tables[0].Rows[0]["Fromdate"]) == Convert.ToDateTime(dsleave.Tables[0].Rows[0]["Todate"]))
    //                {
    //                    if (dsleave.Tables[0].Rows[0]["no_of_day"].ToString() == "1")
    //                    {
    //                        if (dsleave.Tables[0].Rows[0]["admin_access"].ToString() == "2")
    //                        {
    //                            retStr = "LeaveApproved";
    //                        }
    //                        else
    //                        {
    //                            if (dsleave.Tables[0].Rows[0]["isapproved"].ToString() == "3")
    //                            {
    //                                retStr = "LeaveunApproved";
    //                            }
    //                            else
    //                            {
    //                                retStr = "LeaveunApproved";
    //                            }
    //                        }
    //                    }
    //                    else if (dsleave.Tables[0].Rows[0]["no_of_day"].ToString() == "0.5")
    //                    {
    //                        if (dsleave.Tables[0].Rows[0]["admin_access"].ToString() == "2")
    //                        {
    //                            retStr = "HalfDayLeaveApproved";
    //                        }
    //                        else
    //                        {
    //                            if (dsleave.Tables[0].Rows[0]["isapproved"].ToString() == "3")
    //                            {
    //                                retStr = "HalfDayLeaveunApproved";
    //                            }
    //                            else
    //                            {
    //                                retStr = "HalfDayLeaveunApproved";
    //                            }
    //                        }

    //                        // retStr += "<br/><a href=Leaveform.aspx>Apply For Half Day Leave</a>";
    //                    }
    //                }
    //                else
    //                {

    //                    if (dsleave.Tables[0].Rows[0]["admin_access"].ToString() == "2")
    //                    {
    //                        retStr = "LeaveApproved";
    //                    }
    //                    else
    //                    {
    //                        if (dsleave.Tables[0].Rows[0]["isapproved"].ToString() == "3")
    //                        {
    //                            retStr = "LeaveunApproved";
    //                        }
    //                        else
    //                        {
    //                            retStr = "LeaveunApproved";
    //                        }
    //                    }


    //                }
    //            }
    //            string strod = "select * from tbl_ondutydetails where tbl_ondutydetails.status=1 ";
    //            strod += " and tbl_ondutydetails.archive=0 and tbl_ondutydetails.uid='" + empid + "' and ";
    //            strod += "  CAST ('" + date + "' as date) between CAST (tbl_ondutydetails.Fromdate as date)   and CAST (tbl_ondutydetails.Todate as date) and ishrapprove=2";



    //            DataSet dsod = da.GetDataSetInline(strod);
    //            if (dsod.Tables[0].Rows.Count > 0)
    //            {

    //                if (dsod.Tables[0].Rows[0]["ishrapprove"].ToString() == "2")
    //                {
    //                    retStr = "ODApproved";
    //                }
    //                else if (dsod.Tables[0].Rows[0]["ishrapprove"].ToString() == "3")
    //                {
    //                    retStr = "ODunApproved";

    //                }
    //            }
    //            else
    //            {
    //                string strod1 = "select * from tbl_ondutydetails where tbl_ondutydetails.status=1 ";
    //                strod1 += " and tbl_ondutydetails.archive=0 and tbl_ondutydetails.uid='" + empid + "' and ";
    //                strod1 += "   CAST ('" + date + "' as date) between CAST (tbl_ondutydetails.Fromdate as date)   and CAST (tbl_ondutydetails.Todate as date) ";
    //                DataSet dsod1 = da.GetDataSetInline(strod1);
    //                if (dsod1.Tables[0].Rows.Count > 0)
    //                {

    //                    if (dsod1.Tables[0].Rows[0]["ishrapprove"].ToString() == "2")
    //                    {
    //                        retStr = "ODApproved";
    //                    }
    //                    else if (dsod1.Tables[0].Rows[0]["ishrapprove"].ToString() == "3")
    //                    {
    //                        retStr = "ODunApproved";

    //                    }
    //                }
    //            }

    //        }
    //    }
    //    else
    //    {
    //        //SqlParameter[] sqlparam = new SqlParameter[2];
    //        //sqlparam[0] = new SqlParameter("@date", date);
    //        //sqlparam[1] = new SqlParameter("@uid", empid);
    //        ////DataSet ds = da.GetDataSet("[dbo].[Hariyali_SelectHoliday]", sqlparam);

    //        //DataSet ds = da.GetDataSet("[dbo].[Hariyali_SelectHoliday_new]", sqlparam);
    //        //if (Convert.ToInt32(ds.Tables[0].Rows[0]["RCount"]) > 0)
    //        //    retStr = "Holiday";
    //        //else if (showweekoff(Convert.ToDateTime(date), empid))
    //        //{
    //        //    retStr = "WeekOff";
    //        //}

    //        if (Convert.ToDateTime(date).DayOfWeek.ToString() == "Sunday")
    //        {
    //            retStr = "MisPunch";
    //        }
    //        else
    //        {
    //            //DataAccess daaa = new DataAccess();

    //            string strod = "select * from tbl_ondutydetails where tbl_ondutydetails.status=1 ";
    //            strod += " and tbl_ondutydetails.archive=0 and tbl_ondutydetails.uid='" + empid + "' and ";
    //            strod += "  CAST ('" + date + "' as date) between CAST (tbl_ondutydetails.Fromdate as date)   and CAST (tbl_ondutydetails.Todate as date) and ishrapprove=2";
    //            DataSet dsod = daaa.GetDataSetInline(strod);
    //            if (dsod.Tables[0].Rows.Count > 0)
    //            {

    //                if (dsod.Tables[0].Rows[0]["ishrapprove"].ToString() == "2")
    //                {
    //                    retStr = "ODApproved";
    //                }
    //                else if (dsod.Tables[0].Rows[0]["ishrapprove"].ToString() == "3")
    //                {
    //                    retStr = "ODunApproved";

    //                }
    //            }
    //            else
    //            {

    //                string strod1 = "select * from tbl_ondutydetails where tbl_ondutydetails.status=1 ";
    //                strod1 += " and tbl_ondutydetails.archive=0 and tbl_ondutydetails.uid='" + empid + "' and ";
    //                strod1 += "   CAST ('" + date + "' as date) between CAST (tbl_ondutydetails.Fromdate as date)   and CAST (tbl_ondutydetails.Todate as date) ";
    //                DataSet dsod1 = daaa.GetDataSetInline(strod1);
    //                if (dsod1.Tables[0].Rows.Count > 0)
    //                {

    //                    if (dsod1.Tables[0].Rows[0]["ishrapprove"].ToString() == "2")
    //                    {
    //                        retStr = "ODApproved";
    //                    }
    //                    else if (dsod1.Tables[0].Rows[0]["ishrapprove"].ToString() == "3")
    //                    {
    //                        retStr = "ODunApproved";

    //                    }
    //                }
    //                else
    //                {
    //                    retStr = "MisPunch";
    //                }
    //            }




    //        }
    //    }

    //    return retStr;
    //}


    public string GetStatusold(string date, string sDate, string eDate, string empid)
    {
        string retStr = "";
        if (sDate != string.Empty && eDate != string.Empty)
        {
            retStr = "Present";
            string[] da = date.Split(' ');
            string csDate = da[0] + " 12:00:00 PM";
            string ceDate = da[0] + " 4:00:00 PM";

            string lsDate = da[0] + " 9:33:00 AM";
            string leDate = da[0] + " 6:00:00 PM";
            if (Convert.ToDateTime(date).DayOfWeek.ToString() == "Sunday")
            {
                retStr = "PresentWO";
            }
            else
            {
                if (Convert.ToDateTime(sDate) > Convert.ToDateTime(lsDate) || Convert.ToDateTime(eDate) < Convert.ToDateTime(leDate))
                {
                    retStr = "PresentLate";
                }

                if (Convert.ToDateTime(sDate) > Convert.ToDateTime(csDate) || Convert.ToDateTime(eDate) < Convert.ToDateTime(ceDate))
                {
                    retStr = "HalfDay";



                    DataAccess daaa = new DataAccess();
                    string str = "select * from sm_emp_leaves_details_half where sm_emp_leaves_details_half.status=1 ";
                    str += " and sm_emp_leaves_details_half.archive=0 and sm_emp_leaves_details_half.uid='" + empid + "' and ";
                    str += "  '" + date + "' between cast(sm_emp_leaves_details_half.Fromdate as date)   and cast(sm_emp_leaves_details_half.Todate as date) and is_admincancel is null";
                    DataSet dsleave = daaa.GetDataSetInline(str);
                    if (dsleave.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToDateTime(dsleave.Tables[0].Rows[0]["Fromdate"]) == Convert.ToDateTime(dsleave.Tables[0].Rows[0]["Todate"]))
                        {
                            if (dsleave.Tables[0].Rows[0]["no_of_day"].ToString() == "0.5")
                            {
                                if (dsleave.Tables[0].Rows[0]["admin_access"].ToString() == "2")
                                {
                                    retStr = "HalfDayLeaveApproved";
                                }
                                else
                                {
                                    retStr = "HalfDayLeaveunApproved";
                                    //if (dsleave.Tables[0].Rows[0]["isapproved"].ToString() == "3")
                                    //{
                                    //    retStr = "HalfDayLeaveAppliedIn Process)";
                                    //}
                                    //else
                                    //{
                                    //    retStr = "HalfDayLeaveApplied(In Process)";
                                    //}
                                }
                            }
                        }
                        else
                        {
                            if (dsleave.Tables[0].Rows[0]["admin_access"].ToString() == "2")
                            {
                                retStr = "HalfDayLeaveApproved";
                            }
                            else
                            {
                                retStr = "HalfDayLeaveApproved";
                            }

                        }

                    }







                    string strod = "select * from tbl_ondutydetails where tbl_ondutydetails.status=1 ";
                    strod += " and tbl_ondutydetails.archive=0 and tbl_ondutydetails.uid='" + empid + "' and ";
                    strod += "  '" + date + "' between tbl_ondutydetails.Fromdate   and tbl_ondutydetails.Todate";
                    DataSet dsod = daaa.GetDataSetInline(strod);
                    if (dsod.Tables[0].Rows.Count > 0)
                    {
                        if (dsod.Tables[0].Rows[0]["ishrapprove"].ToString() == "2")
                        {
                            retStr = "halfODApproved";
                        }
                        else
                        {
                            retStr = "halfODunApproved";
                           
                        }
                    }




                    
                }
            }
        }
        else if (sDate == string.Empty && eDate == string.Empty)
        {
            retStr = "Absent";
            SqlParameter[] sqlparam = new SqlParameter[1];
            sqlparam[0] = new SqlParameter("@date", date);
            DataSet ds = da.GetDataSet("[dbo].[Hariyali_SelectHoliday]", sqlparam);
            if (Convert.ToInt32(ds.Tables[0].Rows[0]["RCount"]) > 0)
                retStr = "Holiday";
            else if (Convert.ToDateTime(date).DayOfWeek.ToString() == "Sunday")
            {
                retStr = "WeekOff";

                string str = "select * from sm_emp_leaves_details_half where sm_emp_leaves_details_half.status=1 ";
                str += " and sm_emp_leaves_details_half.archive=0 and sm_emp_leaves_details_half.uid='" + empid + "' and ";
                str += "  '" + date + "' between sm_emp_leaves_details_half.Fromdate   and sm_emp_leaves_details_half.Todate and is_admincancel is null";
                DataSet dsleave = da.GetDataSetInline(str);
                if (dsleave.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToDateTime(dsleave.Tables[0].Rows[0]["Fromdate"]) == Convert.ToDateTime(dsleave.Tables[0].Rows[0]["Todate"]))
                    {
                        if (dsleave.Tables[0].Rows[0]["no_of_day"].ToString() == "1")
                        {
                            if (dsleave.Tables[0].Rows[0]["admin_access"].ToString() == "2")
                            {
                                retStr = "LeaveApproved";
                            }
                            else
                            {
                                if (dsleave.Tables[0].Rows[0]["isapproved"].ToString() == "3")
                                {
                                    retStr = "LeaveunApproved";
                                }
                                else
                                {
                                    retStr = "LeaveunApproved";
                                }
                            }
                        }

                    }
                    else
                    {

                        if (dsleave.Tables[0].Rows[0]["admin_access"].ToString() == "2")
                        {
                            retStr = "LeaveApproved";
                        }
                        else
                        {
                            if (dsleave.Tables[0].Rows[0]["isapproved"].ToString() == "3")
                            {
                                retStr = "LeaveunApproved";
                            }
                            else
                            {
                                retStr = "LeaveunApproved";
                            }
                        }


                    }
                }

            }
            else if (Convert.ToDateTime(date).DayOfWeek.ToString() == "Saturday" && Convert.ToDateTime(date).Day > 7 && Convert.ToDateTime(date).Day < 15)
            {
                retStr = "WeekOff";

                string str = "select * from sm_emp_leaves_details_half where sm_emp_leaves_details_half.status=1 ";
                str += " and sm_emp_leaves_details_half.archive=0 and sm_emp_leaves_details_half.uid='" + empid + "' and ";
                str += "  '" + date + "' between sm_emp_leaves_details_half.Fromdate   and sm_emp_leaves_details_half.Todate and is_admincancel is null";
                DataSet dsleave = da.GetDataSetInline(str);
                if (dsleave.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToDateTime(dsleave.Tables[0].Rows[0]["Fromdate"]) == Convert.ToDateTime(dsleave.Tables[0].Rows[0]["Todate"]))
                    {
                        if (dsleave.Tables[0].Rows[0]["no_of_day"].ToString() == "1")
                        {
                            if (dsleave.Tables[0].Rows[0]["admin_access"].ToString() == "2")
                            {
                                retStr = "LeaveApproved";
                            }
                            else
                            {
                                if (dsleave.Tables[0].Rows[0]["isapproved"].ToString() == "3")
                                {
                                    retStr = "LeaveunApproved";
                                }
                                else
                                {
                                    retStr = "LeaveunApproved";
                                }
                            }
                        }

                    }
                    else
                    {

                        if (dsleave.Tables[0].Rows[0]["admin_access"].ToString() == "2")
                        {
                            retStr = "LeaveApproved";
                        }
                        else
                        {
                            if (dsleave.Tables[0].Rows[0]["isapproved"].ToString() == "3")
                            {
                                retStr = "LeaveunApproved";
                            }
                            else
                            {
                                retStr = "LeaveunApproved";
                            }
                        }


                    }
                }

            }
            else
            {
                retStr = "Absent";
                string str = "select * from sm_emp_leaves_details_half where sm_emp_leaves_details_half.status=1 ";
                str += " and sm_emp_leaves_details_half.archive=0 and sm_emp_leaves_details_half.uid='" + empid + "' and ";
                str += "  '" + date + "' between cast(sm_emp_leaves_details_half.Fromdate as date)   and cast(sm_emp_leaves_details_half.Todate as date) and is_admincancel is null";
                DataSet dsleave = da.GetDataSetInline(str);
                if (dsleave.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToDateTime(dsleave.Tables[0].Rows[0]["Fromdate"]) == Convert.ToDateTime(dsleave.Tables[0].Rows[0]["Todate"]))
                    {
                        if (dsleave.Tables[0].Rows[0]["no_of_day"].ToString() == "1")
                        {
                            if (dsleave.Tables[0].Rows[0]["admin_access"].ToString() == "2")
                            {
                                retStr = "LeaveApproved";
                            }
                            else
                            {
                                if (dsleave.Tables[0].Rows[0]["isapproved"].ToString() == "3")
                                {
                                    retStr = "LeaveunApproved";
                                }
                                else
                                {
                                    retStr = "LeaveunApproved";
                                }
                            }
                        }

                    }
                    else
                    {

                        if (dsleave.Tables[0].Rows[0]["admin_access"].ToString() == "2")
                        {
                            retStr = "LeaveApproved";
                        }
                        else
                        {
                            if (dsleave.Tables[0].Rows[0]["isapproved"].ToString() == "3")
                            {
                                retStr = "LeaveunApproved";
                            }
                            else
                            {
                                retStr = "LeaveunApproved";
                            }
                        }


                    }
                }


                // retStr = "Absent";
                string strod = "select * from tbl_ondutydetails where tbl_ondutydetails.status=1 ";
                strod += " and tbl_ondutydetails.archive=0 and tbl_ondutydetails.uid='" + empid + "' and ";
                strod += " CAST ('" + date + "' as date) between CAST (tbl_ondutydetails.Fromdate as date)   and CAST (tbl_ondutydetails.Todate as date) and ishrapprove=2";
                DataSet dsod = da.GetDataSetInline(strod);
                if (dsod.Tables[0].Rows.Count > 0)
                {
                    if (dsod.Tables[0].Rows[0]["ishrapprove"].ToString() == "2")
                    {
                        retStr = "ODApproved";
                    }
                    else
                    {
                        retStr = "ODunApproved";

                    }
                }
                else
                {

                    string strod1 = "select * from tbl_ondutydetails where tbl_ondutydetails.status=1 ";
                    strod1 += " and tbl_ondutydetails.archive=0 and tbl_ondutydetails.uid='" + empid + "' and ";
                    strod1 += " CAST ('" + date + "' as date) between CAST (tbl_ondutydetails.Fromdate as date)   and CAST (tbl_ondutydetails.Todate as date) ";
                    DataSet dsod1 = da.GetDataSetInline(strod);
                    if (dsod1.Tables[0].Rows.Count > 0)
                    {
                        if (dsod1.Tables[0].Rows[0]["ishrapprove"].ToString() == "2")
                        {
                            retStr = "ODApproved";
                        }
                        else
                        {
                            retStr = "ODunApproved";

                        }
                    }

                }




            }
        }
        else
        {
            if (Convert.ToDateTime(date).DayOfWeek.ToString() == "Sunday")
            {
                retStr = "MisPunch";
            }
            else
            {
                retStr = "MisPunch";
            }
        }

        return retStr;
    }




    public bool getweeklyoff(DateTime edate, string empid)
    {
        //try
        //{





        int wcount = 0;
        int dno = 0;
        wcount = GetWeekNumber(Convert.ToDateTime(edate));
        dno = GetdayNumber(Convert.ToDateTime(edate));
        string w1, wt1, w2, wt2, w3, wt3, w4, wt4, w5, wt5, w6, wt6, w7, wt7;
        string strret = "";

        SqlParameter[] prmh = new SqlParameter[1];
        prmh[0] = new SqlParameter("@empid", empid);
        //  prmh[1] = new SqlParameter("@cdate", edate);
        DataSet dsh = da.GetDataSet("WeeklyOffPolicybyempid", prmh);
        if (dsh.Tables[0].Rows.Count > 0)
        {

            w1 = dsh.Tables[0].Rows[0]["isweekoff1"].ToString();
            wt1 = dsh.Tables[0].Rows[0]["isweekoff1type"].ToString();
            w2 = dsh.Tables[0].Rows[0]["isweekoff2"].ToString();
            wt2 = dsh.Tables[0].Rows[0]["isweekoff2type"].ToString();
            w3 = dsh.Tables[0].Rows[0]["isweekoff3"].ToString();
            wt3 = dsh.Tables[0].Rows[0]["isweekoff3type"].ToString();
            w4 = dsh.Tables[0].Rows[0]["isweekoff4"].ToString();
            wt4 = dsh.Tables[0].Rows[0]["isweekoff4type"].ToString();
            w5 = dsh.Tables[0].Rows[0]["isweekoff5"].ToString();
            wt5 = dsh.Tables[0].Rows[0]["isweekoff5type"].ToString();
            w6 = dsh.Tables[0].Rows[0]["isweekoff6"].ToString();
            wt6 = dsh.Tables[0].Rows[0]["isweekoff6type"].ToString();
            w7 = dsh.Tables[0].Rows[0]["isweekoff7"].ToString();
            wt7 = dsh.Tables[0].Rows[0]["isweekoff7type"].ToString();
            int returntype = 0;
            if (w1 == "1" && dno.ToString() == "1")
            {
                returntype = getreturnstatus(wt1, wcount);

            }
            if (w2 == "1" && dno.ToString() == "2")
            {
                returntype = getreturnstatus(wt2, wcount);

            }

            if (w3 == "1" && dno.ToString() == "3")
            {
                returntype = getreturnstatus(wt3, wcount);

            }

            if (w4 == "1" && dno.ToString() == "4")
            {
                returntype = getreturnstatus(wt4, wcount);

            }

            if (w5 == "1" && dno.ToString() == "5")
            {
                returntype = getreturnstatus(wt5, wcount);

            }

            if (w6 == "1" && dno.ToString() == "6")
            {
                returntype = getreturnstatus(wt6, wcount);

            }

            if (w7 == "1" && dno.ToString() == "7")
            {
                returntype = getreturnstatus(wt7, wcount);

            }
            if (returntype == 1)
            {
                return true;
            }
        }


        return false;

        //}
        //catch (Exception ex)
        //{
        //    return false;
        //}
    }

    public int getreturnstatus(string wt1chk, int wcountchk)
    {
        if (wt1chk == "1")
        {
            return 1;
        }
        if (wt1chk == "2" && wcountchk == 1)
        {
            return 1;
        }
        if (wt1chk == "3" && wcountchk == 2)
        {
            return 1;
        }
        if (wt1chk == "4" && wcountchk == 3)
        {
            return 1;
        }
        if (wt1chk == "5" && wcountchk == 4)
        {
            return 1;
        }
        if (wt1chk == "6" && wcountchk == 5)
        {
            return 1;
        }
        if (wt1chk == "7" && (wcountchk == 1 || wcountchk == 3))
        {
            return 1;
        }
        if (wt1chk == "8" && (wcountchk == 1 || wcountchk == 3 || wcountchk == 5))
        {
            return 1;
        }
        if (wt1chk == "9" && (wcountchk == 2 || wcountchk == 4))
        {
            return 1;
        }

        return 0;
    }



    public bool getholiday(DateTime edate, string empid)
    {
        try
        {

            // Response.Write(cmf.Decrypt(empid) + "," + edate.ToShortDateString());
            SqlParameter[] prmh = new SqlParameter[2];
            prmh[0] = new SqlParameter("@empid", empid);
            prmh[1] = new SqlParameter("@cdate", edate);
            DataSet dsh = da.GetDataSet("holidaysselectbydate", prmh);
            if (dsh.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                SqlParameter[] prmrh = new SqlParameter[2];
                prmrh[0] = new SqlParameter("@empid", empid);
                prmrh[1] = new SqlParameter("@cdate", edate);
                DataSet dshr = da.GetDataSet("resholidaysselectbydate", prmrh);
                if (dshr.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {

                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public int getleavedetails(DateTime edate, string empid)
    {
        try
        {

            // Response.Write(cmf.Decrypt(empid) + "," + edate.ToShortDateString());
            SqlParameter[] prm1111 = new SqlParameter[3];
            prm1111[0] = new SqlParameter("@uid", empid);
            prm1111[1] = new SqlParameter("@fromdate", edate);
            prm1111[2] = new SqlParameter("@todate", edate);
            DataSet dsempleavebetween = da.GetDataSet("checkleavebetweendate", prm1111);
        
            if (dsempleavebetween.Tables[0].Rows.Count > 0)
            {
                if (dsempleavebetween.Tables[0].Rows[0]["isapprove"].ToString()=="1")
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 0;
            }
        }
        catch (Exception ex)
        {
            return 0; 
        }
    }


    public int getoddetails(DateTime edate, string empid)
    {
        try
        {

            // Response.Write(cmf.Decrypt(empid) + "," + edate.ToShortDateString());
            SqlParameter[] prm1111 = new SqlParameter[3];
            prm1111[0] = new SqlParameter("@uid", empid);
            prm1111[1] = new SqlParameter("@fromdate", edate);
            prm1111[2] = new SqlParameter("@todate", edate);
            DataSet dsempleavebetween = da.GetDataSet("checkodbetweendate", prm1111);

            if (dsempleavebetween.Tables[0].Rows.Count > 0)
            {
                if (dsempleavebetween.Tables[0].Rows[0]["isapprove"].ToString() == "1")
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 0;
            }
        }
        catch (Exception ex)
        {
            return 0;
        }
    }

    public int getwfhdetails(DateTime edate, string empid)
    {
        try
        {

            // Response.Write(cmf.Decrypt(empid) + "," + edate.ToShortDateString());
            SqlParameter[] prm1111 = new SqlParameter[3];
            prm1111[0] = new SqlParameter("@uid", empid);
            prm1111[1] = new SqlParameter("@fromdate", edate);
            prm1111[2] = new SqlParameter("@todate", edate);
            DataSet dsempleavebetween = da.GetDataSet("checkwfhbetweendate", prm1111);

            if (dsempleavebetween.Tables[0].Rows.Count > 0)
            {
                if (dsempleavebetween.Tables[0].Rows[0]["isapprove"].ToString() == "1")
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 0;
            }
        }
        catch (Exception ex)
        {
            return 0;
        }
    }


    public int getattlogdetails(DateTime edate, string empid)
    {
        try
        {

            // Response.Write(cmf.Decrypt(empid) + "," + edate.ToShortDateString());
            SqlParameter[] prm1111 = new SqlParameter[3];
            prm1111[0] = new SqlParameter("@uid", empid);
            prm1111[1] = new SqlParameter("@fromdate", edate);
            prm1111[2] = new SqlParameter("@todate", edate);
            DataSet dsempleavebetween = da.GetDataSet("checkattlogbetweendate", prm1111);

            if (dsempleavebetween.Tables[0].Rows.Count > 0)
            {
                if (dsempleavebetween.Tables[0].Rows[0]["isapprove"].ToString() == "1")
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 0;
            }
        }
        catch (Exception ex)
        {
            return 0;
        }
    }


}