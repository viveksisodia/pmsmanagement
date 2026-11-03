<%@ Page Title="" Language="C#" MasterPageFile="~/pms/popupmaster.master" AutoEventWireup="true" CodeFile="addrolesresponsibility.aspx.cs" Inherits="settings_adddesignation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
            <div class="acitivity-timeline p-3">
                <div class="row">
                    <div class="col-lg-12">
                        <div class="head_sec">
                            <div class="row">
                                <div class="col-sm-auto">
                                    <h5 class="mb-0">Create Roles & Responsibility</h5>
                                </div>
                               <%-- <div class="col-sm text-right">
                                    <a href="javascript:void(0);" class="btn btn-primary btn-sm addbtn">+ Add New</a>
                                </div>--%>
                            </div>
                        </div>
                        <div class="lead_form_body mb-0 border p-2 border-top-0">
                            <div class="row">
                                <div class="col-sm-5">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Roles & Responsibility Name<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ForeColor="Red" ControlToValidate="Roles_Responsibility_Name" ValidationGroup="gi" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                     <asp:TextBox runat="server" ID="Roles_Responsibility_Name" placeholder="Roles & Responsibility Name" class="form-control"></asp:TextBox>
                                </div>
                                     <div class="col-sm-5">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Description<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ForeColor="Red" ControlToValidate="Roles_Responsibility_Code" ValidationGroup="gi" ErrorMessage="*"></asp:RequiredFieldValidator></label>
                                    </div>
                                     <asp:TextBox runat="server" ID="Roles_Responsibility_Code" placeholder="Description" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">&nbsp;</label>
                                    </div>
                                    <div id="pnlimg" style="display: none;">
                                <img src="/Image/loading8.gif" runat="server" id="img1" />
                            </div>
                            <div id="divsub" style="display:block;">
                            <asp:Button ID="btncreate" runat="server" class="btn btn-primary btn-create" Text="Create" OnClick="btncreate_Click" ValidationGroup="gi" />
                                  </div>
                                </div>
                            </div>
                        </div>

                        <div class="table-responsive mt-3">

                             <asp:GridView ID="GrdvDepartment" runat="server" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" DataKeyNames="id"
                        CssClass="table table-hover table-striped table-sm table-nowrap mb-0 datatable table-bordered" OnPreRender="GrdvDepartment_PreRender">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate><%=fns.getltext("S.No.", "2")%></HeaderTemplate>
                                <ItemTemplate>
                                    <%# Container.DisplayIndex + 1 + ((GrdvDepartment.PageIndex) * (GrdvDepartment.PageSize))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate><%=fns.getltext("Roles & Responsibility Name", "2")%></HeaderTemplate>

                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%#Bind("profile") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate><%=fns.getltext("Description", "2")%></HeaderTemplate>

                                <ItemTemplate>
                                    <asp:Label ID="lblcodee" runat="server" Text='<%#Bind("Description") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate><%=fns.getltext("Action", "2")%></HeaderTemplate>
                                <ItemTemplate>
                                        <asp:Panel ID="Panel1" runat="server" Visible='<%#(Eval("id").ToString()=="1"?false:true)%>'>
                                    <%-- <a  href="javascript:void(0);" class="addbtn">ADD</a>--%>
                                    <asp:LinkButton ID="lnkedit" runat="server" OnClick="lnkedit_Click" CssClass="btn btn-info btn-sm" ToolTip="Edit">  <i class="bi bi-pencil"></i></asp:LinkButton>&nbsp &nbsp
            <asp:LinkButton ID="lnkDelete" runat="server" OnClick="lnkDelete_Click" OnClientClick="javascript:return confirm('Do you really want to \ndelete the item?');" CssClass="btn  btn-danger btn-sm" ToolTip="Delete">  <i class="bi bi-trash"></i></asp:LinkButton>
                               </asp:Panel>
                                            
                                            </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <EmptyDataTemplate>
                            No record available                    
                        </EmptyDataTemplate>
                    </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
</asp:Content>

