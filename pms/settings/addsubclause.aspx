<%@ Page Title="" Language="C#" MasterPageFile="~/pms/popupmaster.master" AutoEventWireup="true" CodeFile="addsubclause.aspx.cs" Inherits="settings_adddesignation" %>

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
                                <div class="col-sm-6">
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
                                    <%--<div class="row">
                                        <div class="col-sm-12">
                                            <input type="text" class="form-control" placeholder="Standard Name">
                                        </div>
                                    </div>--%>
                                </div>
                                <div class="col-sm-6">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Select Clause</label>
                                    </div>
                                    <select class="form-control">
                                        <option>-- Select Clause --</option>
                                        <option>1. Scope</option>
                                        <option>2. Normative Reference</option>
                                        <option>3. Terms and Definition</option>
                                        <option>4. Context of Organisation</option>
                                        <option>5. Leadership</option>
                                        <option>6. Planning</option>
                                        <option>7. Support</option>
                                        <option>8. Operation</option>
                                        <option>9. Performance Evaluation</option>
                                        <option>10. Improvement</option>
                                        <option>+ Add New</option>
                                    </select>
                                    <%--<div class="row">
                                        <div class="col-sm-3">
                                            <input type="text" class="form-control" placeholder="Clause Code">
                                        </div>
                                        <div class="col-sm-9">
                                            <input type="text" class="form-control" placeholder="Clause Name">
                                        </div>
                                    </div>--%>
                                </div>
                                <div class="col-sm-3">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Sub Clause Code</label>
                                    </div>
                                    <input type="text" class="form-control" placeholder="Sub Clause Code">
                                </div>
                                <div class="col-sm-7">
                                    <div class="d-flex align-items-center">
                                        <label class="form-label flex-grow-1">Sub Clause Name</label>
                                    </div>
                                    <input type="text" class="form-control" placeholder="Sub Clause Name">
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
                                        <td><b>4. Context of Organisation</b>
                                            <table>
                                                <tr>
                                                    <td>4.1 Understanding the Company and its contexts</td>
                                                </tr>
                                                <tr>
                                                    <td>4.2 Understanding the needs and expectations of interested parties</td>
                                                </tr>
                                                <tr>
                                                    <td>4.3 Determining the scope of the Quality</td>
                                                </tr>
                                                <tr>
                                                    <td>4.4 Quality and its processes</td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>5</td>
                                        <td>ISO 9001:2015</td>
                                        <td><b>5. Leadership</b>
                                            <table>
                                                <tr>
                                                    <td>5.1 Leadership and commitment</td>
                                                </tr>
                                                <tr>
                                                    <td>5.2 Policy</td>
                                                </tr>
                                                <tr>
                                                    <td>5.3 Roles, responsibility & authority</td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>6</td>
                                        <td>ISO 9001:2015</td>
                                        <td><b>6. Planning</b>
                                            <table>
                                                <tr>
                                                    <td>6.1 Actions to address risks and opportunities</td>
                                                </tr>
                                                <tr>
                                                    <td>6.2 Quality objectives and planning to achieve them</td>
                                                </tr>
                                                <tr>
                                                    <td>6.3 Planning of Changes</td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>7</td>
                                        <td>ISO 9001:2015</td>
                                        <td><b>7. Support</b>
                                            <table>
                                                <tr>
                                                    <td>7.1 Resources</td>
                                                </tr>
                                                <tr>
                                                    <td>7.2 Competence</td>
                                                </tr>
                                                <tr>
                                                    <td>7.3 Awareness</td>
                                                </tr>
                                                <tr>
                                                    <td>7.4 Communication</td>
                                                </tr>
                                                <tr>
                                                    <td>7.5 Document Information</td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>8</td>
                                        <td>ISO 9001:2015</td>
                                        <td><b>8. Operation</b>
                                            <table>
                                                <tr>
                                                    <td>8.1 Operational planning and control</td>
                                                </tr>
                                                <tr>
                                                    <td>8.2 Requirements for products and Product</td>
                                                </tr>
                                                <tr>
                                                    <td>8.3 Design & development of product and Product</td>
                                                </tr>
                                                <tr>
                                                    <td>8.4 Control of externally processes, products and Product</td>
                                                </tr>
                                                <tr>
                                                    <td>8.5 Production and Product provision</td>
                                                </tr>
                                                <tr>
                                                    <td>8.6 Release of products and Product</td>
                                                </tr>
                                                <tr>
                                                    <td>8.7 Control of nonconforming outputs</td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>9</td>
                                        <td>ISO 9001:2015</td>
                                        <td><b>9. Performance Evaluation</b>
                                            <table>
                                                <tr>
                                                    <td>9.1 Monitoring, Measurement, analysis and evaluation</td>
                                                </tr>
                                                <tr>
                                                    <td>9.2 Internal Audit</td>
                                                </tr>
                                                <tr>
                                                    <td>9.3 Management Review</td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>10</td>
                                        <td>ISO 9001:2015</td>
                                        <td><b>10. Improvement</b>
                                            <table>
                                                <tr>
                                                    <td>10.1 General</td>
                                                </tr>
                                                <tr>
                                                    <td>10.2 Nonconformity and corrective action</td>
                                                </tr>
                                                <tr>
                                                    <td>10.3 Continual Improvement</td>
                                                </tr>
                                            </table>
                                        </td>
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
                                        <td><b>4. Context of Organisation</b>
                                            <table>
                                                <tr>
                                                    <td>4.1 Understanding the Company and its contexts</td>
                                                </tr>
                                                <tr>
                                                    <td>4.2 Understanding the needs and expectations of interested parties of interested parties</td>
                                                </tr>
                                                <tr>
                                                    <td>4.3 Determining the scope of the EMS</td>
                                                </tr>
                                                <tr>
                                                    <td>4.4 Environmental Management System</td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>15</td>
                                        <td>ISO 14001:2015</td>
                                        <td><b>5. Leadership</b>
                                            <table>
                                                <tr>
                                                    <td>5.1 Leadership and commitment</td>
                                                </tr>
                                                <tr>
                                                    <td>5.2 EMS Policy</td>
                                                </tr>
                                                <tr>
                                                    <td>5.3 Organization Roles, responsibility & authority</td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>16</td>
                                        <td>ISO 14001:2015</td>
                                        <td><b>6. Planning</b>
                                            <table>
                                                <tr>
                                                    <td>6.1 Actions to address risks and opportunities</td>
                                                </tr>
                                                <tr>
                                                    <td>6.2 EMS objectives and planning to achieve them</td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>17</td>
                                        <td>ISO 14001:2015</td>
                                        <td><b>7. Support</b>
                                            <table>
                                                <tr>
                                                    <td>7.1 Resources</td>
                                                </tr>
                                                <tr>
                                                    <td>7.2 Competence</td>
                                                </tr>
                                                <tr>
                                                    <td>7.3 Awareness</td>
                                                </tr>
                                                <tr>
                                                    <td>7.4 Communication</td>
                                                </tr>
                                                <tr>
                                                    <td>7.5 Document Information</td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>18</td>
                                        <td>ISO 14001:2015</td>
                                        <td><b>8. Operation</b>
                                            <table>
                                                <tr>
                                                    <td>8.1 Operational planning and control</td>
                                                </tr>
                                                <tr>
                                                    <td>8.2 Emergency Preparedness & Response</td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>19</td>
                                        <td>ISO 14001:2015</td>
                                        <td><b>9. Performance Evaluation</b>
                                            <table>
                                                <tr>
                                                    <td>9.1 Monitoring, Measurement, analysis and evaluation</td>
                                                </tr>
                                                <tr>
                                                    <td>9.2 Internal Audit</td>
                                                </tr>
                                                <tr>
                                                    <td>9.3 Management Review</td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <a href="javascript:void(0);" class="btn btn-info btn-sm"><i class="bi bi-pencil"></i></a>
                                            <a href="javascript:void(0);" class="btn btn-danger btn-sm "><i class="bi bi-trash"></i></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>20</td>
                                        <td>ISO 14001:2015</td>
                                        <td><b>10. Improvement</b>
                                            <table>
                                                <tr>
                                                    <td>10.1 General</td>
                                                </tr>
                                                <tr>
                                                    <td>10.2 Nonconformity and corrective action</td>
                                                </tr>
                                                <tr>
                                                    <td>10.3 Continual Improvement</td>
                                                </tr>
                                            </table>
                                        </td>
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

