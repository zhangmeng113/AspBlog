﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Configuration;

public partial class articleDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strConnection = WebConfigurationManager.ConnectionStrings["BlogConnectionString"].ConnectionString.ToString();
        SqlConnection Connection = new SqlConnection(strConnection);
        String strSQL = "select * from Articles where ArticleID=@ArticleID";
        SqlCommand command = new SqlCommand(strSQL, Connection);

        command.Parameters.AddWithValue("@ArticleID", 3);
        Connection.Open();
        SqlDataReader sqlDataReader = command.ExecuteReader();

        while (sqlDataReader.Read())
        {
            int num = sqlDataReader.GetInt32(4);
            String type = getType(num);
            Label1.Text += sqlDataReader.GetString(1) + "";
            Label2.Text += "zl";
            Label3.Text += type + "";
            Label4.Text += sqlDataReader.GetString(3) + "";
            Label5.Text += Application["total"].ToString() + "";
            Literal innerHtml = new Literal();
            innerHtml.Text = sqlDataReader.GetString(2) + "";
            Panel1.Controls.Add(innerHtml);
        }
    }
    //获取类型
    public String getType(int num)
    {
        String type="";
        if (num == 1)
        {
            type = "科技";
        }
        else if(num ==2)
        {
            type = "情感";
        }
        else if(num ==3)
        {
            type = "生活";
        }
        return type;
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string aid = "111";//Session["ArticleID"].ToString();
        string name = "111";//Session["UserName"].ToString();



        if (TextBox1.Text == "")
            Response.Write("<script>alert('评论不能为空!')</script>");
        else
        {



            string sqlconnstr = ConfigurationManager.ConnectionStrings["BlogConnectionString"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(sqlconnstr);
            //建立DataSet对象
            DataSet ds = new DataSet();
            //建立DataTable对象
            DataTable dtable;
            //建立DataRowCollection对象
            DataRowCollection coldrow;
            //建立DataRow对象
            DataRow drow;
            //打开连接
            sqlconn.Open();
            //建立DataAdapter对象
            SqlDataAdapter sqld = new SqlDataAdapter("select * from Comments", sqlconn);
            //自己定义Update命令，其中@NAME，@NO是两个参数

            SqlCommandBuilder cb = new SqlCommandBuilder(sqld);

            //用Fill方法返回的数据，填充DataSet，数据表取名为“tabstudent”
            sqld.Fill(ds, "tabstudent");
            //将数据表tabstudent的数据复制到DataTable对象
            dtable = ds.Tables["tabstudent"];
            //用DataRowCollection对象获取这个数据表的所有数据行
            coldrow = dtable.Rows;
            //修改操作，逐行遍历，取出各行的数据
            int n;
            int Cmtid;
            n = coldrow.Count;
            if (n >= 1)
            {
                drow = coldrow[n - 1];
                Cmtid = (Convert.ToInt32(drow[0]) + 1);
            }
            else
                Cmtid = 1;
            drow = ds.Tables["tabstudent"].NewRow();
            drow[0] = Cmtid;
            drow[1] = TextBox1.Text;
            drow[2] = name;
            drow[3] = System.DateTime.Now.ToString();
            drow[4] = aid;


            ds.Tables["tabstudent"].Rows.Add(drow);

            //提交更新
            sqld.Update(ds, "tabstudent");

            sqlconn.Close();
            sqlconn = null;
            Response.Redirect("articleDetail.aspx");

        }
    }
}