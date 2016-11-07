﻿
' Instat-R
' Copyright (C) 2015
'
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License k
' along with this program.  If not, see <http://www.gnu.org/licenses/>.

Public Class ucrLayerParamsControls
    Public clsGeomFunction As RFunction

    Public Event RParameterChanged(ucrControl As ucrLayerParamsControls)

    Private Sub InitialiseControl()

    End Sub

    Public Sub SetGeomFunction(clsTempFunction As RFunction)
        clsGeomFunction = clsTempFunction
        RaiseEvent RParameterChanged(Me)
    End Sub

    Public Sub SetLayerParameter(clsTempLayerParam As LayerParameter)
        'This sets up the Layer parameter of the current control. First get the right LayerParameter stored in the ucrReceiverMetadataProperty.
        ucrReceiverMetadataProperty.clsLayerParam = clsTempLayerParam
        'The following sub sets the right form for the ucrReceiverProperty according to the type, default value and other features of the layer parameter (info from clsLayerParam).
        ucrReceiverMetadataProperty.SetControls()
        'Then the parameter's label, ucrReceiverMP, and value are set if clsLayerParameter is non-empty. Otherwise parameter name chk is hiddeN
        If ucrReceiverMetadataProperty.clsLayerParam IsNot Nothing Then
            chkParamName.Visible = True
            chkParamName.Text = ucrReceiverMetadataProperty.clsLayerParam.strLayerParameterName

            'If that parameter has already a value in the clsGeomFunction, it needs to be setup accordingly on the dialog. Otherwise defaults are applied.
            If clsGeomFunction.GetParameter(ucrReceiverMetadataProperty.clsLayerParam.strLayerParameterName) Is Nothing Then
                ucrReceiverMetadataProperty.SetValue(ucrReceiverMetadataProperty.clsLayerParam.strParameterDefaultValue)
                chkParamName.Checked = False
            Else
                ucrReceiverMetadataProperty.SetValue(clsGeomFunction.GetParameter(ucrReceiverMetadataProperty.clsLayerParam.strLayerParameterName).strArgumentValue)
                chkParamName.Checked = True
            End If
            'If the parameter is checked, the value it takes should be visible.
            ucrReceiverMPVisible()
            ucrReceiverMetadataProperty.ctrActive.Visible = chkParamName.Checked
        Else
            chkParamName.Visible = False
        End If

    End Sub

    Private Sub ucrReceiverMPVisible()
        ucrReceiverMetadataProperty.Visible = chkParamName.Checked
    End Sub
    Private Sub chkParamName_CheckedChanged(sender As Object, e As EventArgs) Handles chkParamName.CheckedChanged
        ucrReceiverMPVisible()
        ucrReceiverMetadataProperty.ctrActive.Visible = True
        RaiseEvent RParameterChanged(Me)
    End Sub

    Private Sub ucrLayerParamsControls_RParameterChanged(ucrControl As ucrLayerParamsControls) Handles Me.RParameterChanged
        If Not IsNothing(ucrReceiverMetadataProperty.clsLayerParam) AndAlso ucrReceiverMetadataProperty.ctrActive IsNot Nothing AndAlso Not IsNothing(clsGeomFunction) Then
            If chkParamName.Checked AndAlso ucrReceiverMetadataProperty.ctrActive.Text <> "" Then
                clsGeomFunction.AddParameter(ucrReceiverMetadataProperty.clsLayerParam.strLayerParameterName, ucrReceiverMetadataProperty.ctrActive.Text)
            Else
                clsGeomFunction.RemoveParameterByName(ucrReceiverMetadataProperty.clsLayerParam.strLayerParameterName)
            End If
        End If
    End Sub

    Private Sub ucrReceiverMetadataProperty_ControlContentsChanged() Handles ucrReceiverMetadataProperty.ControlContentsChanged
        RaiseEvent RParameterChanged(Me)
    End Sub
End Class