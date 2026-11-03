using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CommonClass;
using System.Collections.Generic;
using System.IO;
using CustomDropDown;
using System.Web.Services;
using System.Web.Script.Services;
public partial class GanttData : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetGanttData(string pid)
    {
        int chkadmin = 0;
        function fns = new function();
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        string userid = HttpContext.Current.Request.Cookies["user"]["user_id"].ToString();
        try
        {
            if (HttpContext.Current.Request.QueryString["empid"] != null)
            {
                userid = cmf.Decrypt(HttpContext.Current.Request.QueryString["empid"]).ToString();
            }
        }
        catch
        { }
        chkadmin = checkadmin(userid);
        var tasks = new List<object>();
        var links = new List<object>();
        string strproject = "select * from Pms_tbl_AddProject where id='" + pid + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";

        DataSet dsP = da.GetDataSetInline(strproject);
        if (dsP.Tables[0].Rows.Count > 0)
        {
            string strsqlM = "select ('M'+cast(Pms_tbl_AddMilestone.id as varchar)) id,id mid,Milestone text,cast(isnull(StartDate,GETDATE()) as date) start_date,cast(isnull(EndDate,GETDATE()) as date) end_date,ISNULL(TaskDuration, 0) duration,cast(ROUND(isnull(iscompletepercentage,0),2) AS DECIMAL(10,2)) completed,'0' progress ,isnull(Weightage,0) Weightage,isnull(statuscomplete,0) statuscomplete from Pms_tbl_AddMilestone where ProjectId='" + pid + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by isnull(orderno,0)";
            int linkIdCounter = 1;
            DataSet dsM = da.GetDataSetInline(strsqlM);
            if (dsM.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsM.Tables[0].Rows.Count; i++)
                {
                    string progressbar = "0";
                    double chkval = 0;
                    if (dsM.Tables[0].Rows[i]["statuscomplete"].ToString() == "1")
                    {
                        progressbar = "1";
                    }
                    else
                    {
                        //chkval = GetTaskProgress(dsM.Tables[0].Rows[i]["mid"].ToString());
                        chkval = Convert.ToDouble(dsM.Tables[0].Rows[i]["completed"].ToString());
                        if (chkval > 0)
                        {
                            progressbar = (chkval / 100).ToString();
                        }
                       
                    }
                    string mid = dsM.Tables[0].Rows[i]["id"].ToString();// with M+id
                    string mid2 = dsM.Tables[0].Rows[i]["mid"].ToString();// only id
                    string tid = "";
                    string tid2 = "";
                    string sid = "";
                    string sid2 = "";
                    tasks.Add(new
                    {                    
                        id = dsM.Tables[0].Rows[i]["id"],
                        text = dsM.Tables[0].Rows[i]["text"],
                        start_date = Convert.ToDateTime(dsM.Tables[0].Rows[i]["start_date"]).ToString("dd-MM-yyyy"),
                        end_date = Convert.ToDateTime(dsM.Tables[0].Rows[i]["end_date"]).ToString("dd-MM-yyyy"),
                        duration = dsM.Tables[0].Rows[i]["duration"],
                        progress = progressbar,
                        open = true
                    });
                    try
                    {

                        DataTable dtMD = getdependencydetails(mid2, "milestone");
                        if (dtMD.Rows.Count > 0)
                        {
                            for (int c = 0; c < dtMD.Rows.Count; c++)
                            {
                                if (dtMD.Rows[c]["stid"].ToString() != "" && dtMD.Rows[c]["stid"].ToString() != "")
                                {
                                    links.Add(new
                                    {
                                        id = linkIdCounter++,
                                        source = "S" + dtMD.Rows[c]["stid"].ToString(),
                                        target = mid,
                                        type = 1  // Start to Start
                                    });
                                }
                                else if (dtMD.Rows[c]["tid"].ToString() != "" && dtMD.Rows[c]["tid"].ToString() != "")
                                {
                                    links.Add(new
                                    {
                                        id = linkIdCounter++,
                                        source = "T" + dtMD.Rows[c]["tid"].ToString(),
                                        target = mid,
                                        type = 1  // Start to Start
                                    });
                                }
                                else if (dtMD.Rows[c]["mid"].ToString() != "" && dtMD.Rows[c]["mid"].ToString() != "")
                                {
                                    links.Add(new
                                    {
                                        id = linkIdCounter++,
                                        source = "M" + dtMD.Rows[c]["mid"].ToString(),
                                        target = mid,
                                        type = 1  // Start to Start
                                    });
                                }


                            }

                        }

                    }
                    catch
                    {


                    }
                    string strsqlT = "select ('T'+cast(Pms_tbl_Addtask.id as varchar)) id,id tid,taskname text,cast(isnull(StartDate,GETDATE()) as date) start_date,cast(isnull(StartTime,'00:00:00.000') as time) StartTime,cast(isnull(Targetdate,GETDATE()) as date) end_date,cast(isnull(Timeline,'00:00:00.000') as time) Endtime,ISNULL(TaskDuration, 0) duration,cast(ROUND(isnull(iscompletepercentage,0),2) AS DECIMAL(10,2))  completed,'0' progress,isnull(Weightage,0) Weightage,isnull(statuscomplete,0) statuscomplete from Pms_tbl_Addtask where ProjectId='" + pid + "' and Milestoneid='" + dsM.Tables[0].Rows[i]["mid"] + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by isnull(orderno,0) ";
                    //SaveLogText("strproject-" + strsqlT);
                    DataSet dsT = da.GetDataSetInline(strsqlT);
                    if (dsT.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < dsT.Tables[0].Rows.Count; j++)
                        {
                            progressbar = "0";
                            if (dsT.Tables[0].Rows[j]["statuscomplete"].ToString() == "1")
                            {
                                progressbar = "1";
                            }
                            else
                            {
                                //chkval = GetSubTaskProgress(dsT.Tables[0].Rows[j]["tid"].ToString());
                                chkval = Convert.ToDouble(dsT.Tables[0].Rows[j]["completed"].ToString());
                                if(chkval > 0)
                                {
                                    progressbar = (chkval / 100).ToString();
                                }

                            }
                            string start_datetime = Convert.ToDateTime(dsT.Tables[0].Rows[j]["start_date"].ToString()).ToString("dd-MM-yyyy") + " " + dsT.Tables[0].Rows[j]["StartTime"].ToString();
                            string end_datetime = Convert.ToDateTime(dsT.Tables[0].Rows[j]["end_date"].ToString()).ToString("dd-MM-yyyy") + " " + dsT.Tables[0].Rows[j]["Endtime"].ToString();


                            tid = dsT.Tables[0].Rows[j]["id"].ToString();
                            tid2 = dsT.Tables[0].Rows[j]["tid"].ToString();
                            tasks.Add(new
                            {
                                id = dsT.Tables[0].Rows[j]["id"],
                                text = dsT.Tables[0].Rows[j]["text"],
                                //start_date = Convert.ToDateTime(dsT.Tables[0].Rows[j]["start_date"].ToString()).ToString("dd-MM-yyyy"),
                                start_date = start_datetime,
                                end_date = end_datetime,
                                duration = dsT.Tables[0].Rows[j]["duration"],
                                progress = progressbar,
                                parent = dsM.Tables[0].Rows[i]["id"],
                                open = true
                            });
                            links.Add(new
                            {
                                id = linkIdCounter++,
                                source = dsT.Tables[0].Rows[j]["id"],
                                target = dsM.Tables[0].Rows[i]["id"],
                                type = 0
                            });
                            try
                            {

                                DataTable dtTD = getdependencydetails(tid2, "task");
                                if (dtTD.Rows.Count > 0)
                                {
                                    for (int c = 0; c < dtTD.Rows.Count; c++)
                                    {
                                        if (dtTD.Rows[c]["stid"].ToString() != "" && dtTD.Rows[c]["stid"].ToString() != "")
                                        {
                                            links.Add(new
                                            {
                                                id = linkIdCounter++,
                                                source = "S" + dtTD.Rows[c]["stid"].ToString(),
                                                target = tid,
                                                type = 1  // Start to Start
                                            });
                                        }
                                        else if (dtTD.Rows[c]["tid"].ToString() != "" && dtTD.Rows[c]["tid"].ToString() != "")
                                        {
                                            links.Add(new
                                            {
                                                id = linkIdCounter++,
                                                source = "T" + dtTD.Rows[c]["tid"].ToString(),
                                                target = tid,
                                                type = 1  // Start to Start
                                            });
                                        }
                                        else if (dtTD.Rows[c]["mid"].ToString() != "" && dtTD.Rows[c]["mid"].ToString() != "")
                                        {
                                            links.Add(new
                                            {
                                                id = linkIdCounter++,
                                                source = "M" + dtTD.Rows[c]["mid"].ToString(),
                                                target = tid,
                                                type = 1  // Start to Start
                                            });
                                        }


                                    }

                                }

                            }
                            catch
                            {


                            }

                            string strsqlS = "select ('S'+cast(Pms_tbl_Addsubtask.id as varchar)) id ,id sid,Subtaskname text,cast(isnull(StartDate,GETDATE()) as date) start_date,cast(isnull(StartTime,'00:00:00.000') as time) StartTime,cast(isnull(Targetdate,GETDATE()) as date) end_date,cast(isnull(Timeline,'00:00:00.000') as time) Endtime,ISNULL(TaskDuration, 0) duration,cast(ROUND(isnull(iscompletepercentage,0),2) AS DECIMAL(10,2))  completed,'0' progress,isnull(Weightage,0) Weightage,isnull(statuscomplete,0) statuscomplete from Pms_tbl_Addsubtask where task='" + dsT.Tables[0].Rows[j]["tid"].ToString() + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by isnull(orderno,0) ";
                            DataSet dsS = da.GetDataSetInline(strsqlS);
                            if (dsS.Tables[0].Rows.Count > 0)
                            {
                                for (int k = 0; k < dsS.Tables[0].Rows.Count; k++)
                                {
                                    progressbar = "0";
                                    if (dsS.Tables[0].Rows[k]["statuscomplete"].ToString() == "1")
                                    {
                                        progressbar = "1";
                                    }
                                    string start_datetimeS = Convert.ToDateTime(dsS.Tables[0].Rows[k]["start_date"].ToString()).ToString("dd-MM-yyyy") + " " + dsS.Tables[0].Rows[k]["StartTime"].ToString();
                                    string end_datetimeS = Convert.ToDateTime(dsS.Tables[0].Rows[k]["end_date"].ToString()).ToString("dd-MM-yyyy") + " " + dsS.Tables[0].Rows[k]["Endtime"].ToString();

                                    sid = dsS.Tables[0].Rows[k]["id"].ToString();
                                    sid2 = dsS.Tables[0].Rows[k]["sid"].ToString();
                                    tasks.Add(new
                                    {
                                        id = dsS.Tables[0].Rows[k]["id"],
                                        text = dsS.Tables[0].Rows[k]["text"],
                                        //start_date = Convert.ToDateTime(dsS.Tables[0].Rows[k]["start_date"]).ToString("dd-MM-yyyy"),
                                        start_date = start_datetimeS,
                                        end_date = end_datetimeS,
                                        duration = dsS.Tables[0].Rows[k]["duration"],
                                        progress = progressbar,
                                        parent = dsT.Tables[0].Rows[j]["id"],
                                        open = true
                                    });
                                    links.Add(new
                                    {
                                        id = linkIdCounter++,
                                        source = dsS.Tables[0].Rows[k]["id"],
                                        target = dsT.Tables[0].Rows[j]["id"],
                                        type = 0
                                    });
                                    try
                                    {

                                        DataTable dtSD = getdependencydetails(sid2, "subtask");
                                        if (dtSD.Rows.Count > 0)
                                        {
                                            for (int c = 0; c < dtSD.Rows.Count; c++)
                                            {
                                                if (dtSD.Rows[c]["stid"].ToString() != "" && dtSD.Rows[c]["stid"].ToString() != "")
                                                {
                                                    links.Add(new
                                                    {
                                                        id = linkIdCounter++,
                                                        source = "S" + dtSD.Rows[c]["stid"].ToString(),
                                                        target = sid,
                                                        type = 1
                                                    });
                                                }
                                                else if (dtSD.Rows[c]["tid"].ToString() != "" && dtSD.Rows[c]["tid"].ToString() != "")
                                                {
                                                    links.Add(new
                                                    {
                                                        id = linkIdCounter++,
                                                        source = "T" + dtSD.Rows[c]["tid"].ToString(),
                                                        target = sid,
                                                        type = 1
                                                    });
                                                }
                                                else if (dtSD.Rows[c]["mid"].ToString() != "" && dtSD.Rows[c]["mid"].ToString() != "")
                                                {
                                                    links.Add(new
                                                    {
                                                        id = linkIdCounter++,
                                                        source = "M" + dtSD.Rows[c]["mid"].ToString(),
                                                        target = sid,
                                                        type = 1
                                                    });
                                                }


                                            }

                                        }

                                    }
                                    catch
                                    {


                                    }
                                }

                            }


                        }

                    }
                }
            }
        }



        return new { data = tasks, links = links };
    }
    public static DataTable getdependencydetails(string RecordId = "0", string RecordType = "")
    {
        DataAccess da = new DataAccess();
        string strdependency = "";
        string retval = "";

        string strsql = @"select DM.id,isnull(DM.milestoneid,0) mid,isnull(DM.taskid,0) tid,isnull(DM.subtaskid,0) stid, Prj.ProjectName,dmilestoneid mids,dtaskid tskids,dsubtaskid subtaskids,mil.Milestone,tsk.taskname,stsk.Subtaskname  from tbl_dependencyST DM 
                        left join Pms_tbl_Addsubtask stsk on DM.subtaskid = stsk.id                        
                        left join Pms_tbl_Addtask tsk on DM.taskid = tsk.id                        
                        left join Pms_tbl_AddMilestone mil on DM.milestoneid = mil.id                        
                        left join Pms_tbl_AddProject Prj on DM.projectid = Prj.id                        
                        where DM.status = 1 and DM.archive = 0
                        and DM.regid = '" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' ";

        if (RecordType == "milestone")
        {
            strsql += " and DM.milestoneid='" + RecordId + "'";
        }
        else if (RecordType == "task")
        {

            strsql += " and DM.taskid='" + RecordId + "'";
        }
        else if (RecordType == "subtask")
        {
            strsql += " and DM.subtaskid='" + RecordId + "'";
        }
        //SaveLogText("TasknameDependency-" + strsql);
        DataSet dsdetails = da.GetDataSetInline(strsql);

        if (dsdetails.Tables.Count == 0 || dsdetails.Tables[0].Rows.Count == 0)
        {
            return new DataTable();
        }

        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@id", dsdetails.Tables[0].Rows[0]["id"].ToString());
        prm[1] = new SqlParameter("@regid", HttpContext.Current.Request.Cookies["cadmin"]["id"]);
        DataSet dsD = da.GetDataSet("sp_dependencydetails", prm);




        return dsD.Tables[0];
    }
    
    protected static int checkadmin(string struserid = "0")
    {
        int retval = 0;
        DataAccess da = new DataAccess();
        CommonFunction cmf = new CommonFunction();
        function fns = new function();
        string strsql = "select Manage_Profile.* from Manage_Profile inner join tbl_employee on Manage_Profile.id=cast(tbl_employee.roleid as int) where tbl_employee.id='" + struserid + "'  and Manage_Profile.status=1 and Manage_Profile.archive=0 ";
        DataSet ds = da.GetDataSetInline(strsql);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["id"].ToString() == "1")
            {
                retval = 1;
            }
        }
        return retval;
    }
    public static string SaveLogText(string Message)
    {
        string retval = "";
        CommonFunction cmf1 = new CommonFunction();
        DataAccess da1 = new DataAccess();
        DataSet dschk = new DataSet();

        SqlParameter[] prm = new SqlParameter[2];
        prm[0] = new SqlParameter("@message", Message);
        prm[1] = new SqlParameter("ReturnValue", SqlDbType.Int);
        prm[1].Direction = ParameterDirection.ReturnValue;
        retval = Convert.ToString(da1.executeDMLQuery("crm_entryLogTable", prm));
        return retval;
    }


    public static double GetSubTaskProgress(string tskid)
    {
        DataAccess da1 = new DataAccess();
        double ret = 0;
        string strsqlS1 = "select id,isnull(Weightage,0) Weightage,isnull(statuscomplete,0) statuscomplete from Pms_tbl_Addsubtask where task='" + tskid + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by isnull(orderno,0) ";
        DataSet dsS1 = da1.GetDataSetInline(strsqlS1);
        if (dsS1.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dsS1.Tables[0].Rows.Count; i++)
            {
                if (dsS1.Tables[0].Rows[i]["statuscomplete"].ToString() == "1")
                {
                    ret += Convert.ToDouble(dsS1.Tables[0].Rows[i]["Weightage"].ToString());
                }                
            }
        }
        return ret;
    }

    public static double GetTaskProgress(string mileid)
    {
        DataAccess da1 = new DataAccess();
        double ret = 0;
        string strsqlT1 = "select id,isnull(Weightage,0) Weightage,isnull(statuscomplete,0) statuscomplete from Pms_tbl_Addtask where Milestoneid='" + mileid + "' and status=1 and archive=0 and regid='" + HttpContext.Current.Request.Cookies["cadmin"]["id"] + "' order by isnull(orderno,0) ";
        DataSet dsT1 = da1.GetDataSetInline(strsqlT1);
        if(dsT1.Tables[0].Rows.Count>0)
        {
            for(int i=0;i< dsT1.Tables[0].Rows.Count;i++)
            {
             if(dsT1.Tables[0].Rows[i]["statuscomplete"].ToString()=="1")
                {
                    ret += Convert.ToDouble(dsT1.Tables[0].Rows[i]["Weightage"].ToString());
                }
                else
                {
                    if (GetSubTaskProgress(dsT1.Tables[0].Rows[i]["id"].ToString()) > 0)
                    {
                        ret += GetSubTaskProgress(dsT1.Tables[0].Rows[i]["id"].ToString());
                    }
                }
            }
        }
        //if (task.SubTasks != null && task.SubTasks.Count > 0)
        //{
        //    return GetSubTaskProgress(task.SubTasks);
        //}
        return ret;
    }
    
}
