<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/pms/popupmaster.master" CodeFile="AddTagsPopup.aspx.cs" Inherits="AddTagsPopup" %>
<%@ Register Assembly="DropDownChosen" Namespace="CustomDropDown" TagPrefix="cc1" %>
<%@ Register Src="~/pms/ucontrol/Tags.ascx" TagPrefix="uc1" TagName="Tags" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="offcanvas-header border-bottom" style="padding:14px 24px;">
          <h5 class="offcanvas-title"><%=Projectname%></h5>
        <%-- <h5 class="offcanvas-title">Project Name Here</h5>--%>
        <div class="hstack gap-2 cls_btn">
            <button type="button" id="zoominid" onclick="zoomin()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsr"><i class="ph-arrows-out-simple-bold"></i></button>
            <button type="button" id="zoomoutid" onclick="zoomout()" class="btn btn-success btn-icon btn-sm rounded-circle btn-ios fsre"><i class="ph-arrows-in-simple-bold"></i></button>
            <button type="button" class="btn btn-danger btn-icon btn-sm rounded-circle btn-ios" onclick="closepopupmain()"   aria-label="Close"><i class="ph-x-bold"></i></button>
        </div>
    </div>
    <asp:HiddenField ID="hdiprojid" runat="server" Value="0" />
    <div class="offcanvas-body p-0 overflow-hidden">
         <%--<div data-simplebar style="height: calc(100vh - 57px);">  --%>
        <div style="height: calc(100vh - 57px);">   
            <div class="acitivity-timeline p-0">
                <div class="card border-0">
                  <uc1:Tags runat="server" ID="Tags" />
                </div>
            </div>
      </div>
    </div>
</asp:Content>

