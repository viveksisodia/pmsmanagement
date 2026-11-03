<%@ Page Title="" Language="C#" MasterPageFile="~/pms/popupmaster.master" AutoEventWireup="true" CodeFile="addclause.aspx.cs" Inherits="settings_adddesignation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
            <div class="acitivity-timeline p-3">
                <div class="row">
                    <div class="col-lg-12">
                        <div class="head_sec">
                            <div class="row">
                                <div class="col-sm-auto">
                                    <h5 class="mb-0">Create Clause</h5>
                                </div>
                            </div>
                        </div>
                        <div class="lead_form_body mb-0 border p-2 border-top-0">
                            <div class="row">
                                <div class="col-sm-3">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Select Standard</label>
                                    </div>
                                    <select class="form-control">
                                        <option>-- Select Standard --</option>
                                        <option>ISO 9001:2015</option>
                                        <option>ISO 14001:2015</option>
                                        <option>ISO 45001:2018</option>
                                        <option>ISO 22000:2018</option>
                                        <option>ISO 20000-1:2018</option>
                                        <option>ISO 27001:2022</option>
                                        <option>ISO 13485:2016</option>
                                        <option>ISO 50001:2018</option>
                                        <option>ISO 22301:2019</option>
                                        <option>ISO 37001:2016</option>
                                        <option>HACCP</option>
                                        <option>GMP</option>
                                        <option>FSSC 22000 V6</option>
                                        <option>+ Add New</option>
                                    </select>
                                    <%--<input type="text" class="form-control" placeholder="Standard Name">--%>
                                </div>
                                <div class="col-sm-2">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Clause Code</label>
                                    </div>
                                    <input type="text" class="form-control" placeholder="Clause Code">
                                </div>
                                <div class="col-sm-5">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Clause Name</label>
                                    </div>
                                    <input type="text" class="form-control" placeholder="Clause Name">
                                </div>
                                <div class="col-sm-2">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">&nbsp;</label>
                                    </div>
                                    <a href="#" class="btn btn-primary btn-create">Create</a>
                                </div>
                            </div>
                        </div>

                        <div class="table-responsive mt-3">
                            <table class="table table-hover table-striped table-sm mb-0 datatable table-bordered">
                                <thead>
                                    <tr>
                                        <th width="40">Sr. No.</th>
                                        <th width="100">Standard</th>
                                        <th>Name of Clause</th>
                                        <th width="47">Action</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>1</td>
                                        <td>ISO 9001:2015</td>
                                        <td><b>1. Scope</b></td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>2</td>
                                        <td>ISO 9001:2015</td>
                                        <td><b>2. Normative Reference</b></td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>3</td>
                                        <td>ISO 9001:2015</td>
                                        <td><b>3. Terms and Definition</b></td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>4</td>
                                        <td>ISO 9001:2015</td>
                                        <td><b>4. Context of Organisation</b></td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>5</td>
                                        <td>ISO 9001:2015</td>
                                        <td><b>5. Leadership</b></td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>6</td>
                                        <td>ISO 9001:2015</td>
                                        <td><b>6. Planning</b></td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>7</td>
                                        <td>ISO 9001:2015</td>
                                        <td><b>7. Support</b></td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>8</td>
                                        <td>ISO 9001:2015</td>
                                        <td><b>8. Operation</b></td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>9</td>
                                        <td>ISO 9001:2015</td>
                                        <td><b>9. Performance Evaluation</b></td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>10</td>
                                        <td>ISO 9001:2015</td>
                                        <td><b>10. Improvement</b></td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>11</td>
                                        <td>ISO 14001:2015</td>
                                        <td><b>1. Scope</b></td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>12</td>
                                        <td>ISO 14001:2015</td>
                                        <td><b>2. Normative Reference</b></td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>13</td>
                                        <td>ISO 14001:2015</td>
                                        <td><b>3. Terms and Definition</b></td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>14</td>
                                        <td>ISO 14001:2015</td>
                                        <td><b>4. Context of Organisation</b></td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>15</td>
                                        <td>ISO 14001:2015</td>
                                        <td><b>5. Leadership</b></td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>16</td>
                                        <td>ISO 14001:2015</td>
                                        <td><b>6. Planning</b></td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>17</td>
                                        <td>ISO 14001:2015</td>
                                        <td><b>7. Support</b></td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>18</td>
                                        <td>ISO 14001:2015</td>
                                        <td><b>8. Operation</b></td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>19</td>
                                        <td>ISO 14001:2015</td>
                                        <td><b>9. Performance Evaluation</b></td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>20</td>
                                        <td>ISO 14001:2015</td>
                                        <td><b>10. Improvement</b></td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
</asp:Content>

