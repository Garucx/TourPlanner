   M S B A M L   `   `   `  ����   TourPlanner!    TourPlanner.View.MainWindow   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006       � WindowStartupLocation$  CenterScreen=� � Title$ Tour Planner��$	��500��$
��1000��$	��400��$	��400��.��� -    � ColumnDefinitions �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* � � RowDefinitions �� $��20 ��� $��20 ��� $
��auto ��� $��* ��� $��* �.+��� $��0��$��1��$��3������ *
SearchText+ �� Mode$ TwoWay=��� $��4��$��1��$��Search������ *
TextSearch+&   System.Windows.Controls.DataGrid  -   $��dg_alltours��   AutoGenerateColumns$ False��$��0��$��3��$��3������ *Tour+$ OneWay=� �� SelectedItem �� *SelectedTour+$ TwoWay=�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ * 	   Header$
	 Name��1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 �� *Name+�� $��3��$��1��$��5��.+�� $
��auto��$
��auto�� �� $��* ��� $��* � �� $
��10px ��� $��* ��� $
��10px ��� $��* ��� $
��10px ��� $��* ��� $
��10px ��� $��* ��� $
��10px ��� $��* ��� $
��10px ��� $��* ��� $
��10px ��� $��* ��� $
��10px ��� $��* ��� $
��10px �.+��� $��Title: ��$��1��$��0���� $��Tour Description��$��0��$��3���� $��To��$��0��$��5���� $
��From��$��0��$��7���� $��Transport Type��$��0��$��9���� $��Distance��$��0��$��11���� $
��Time��$��0��$��13���� $��Route Information��$��0��$��15���� $��1��$��1������ *SelectedTour.Name+�� $��1��$��3������ *SelectedTour.Tour_desc+�� $��1��$��5������ *SelectedTour.From+�� $��1��$��7������ *SelectedTour.To+�� $��1��$��9������ *SelectedTour.Transport_type+�� $��1��$��11������ *SelectedTour.Distance+�� $��1��$��13������ *SelectedTour.Time+�� $��1��$��15������ * SelectedTour.Route_information+  -   $��dg_tourlogs��$��1��$��4��$��5������ *SelectedTour.TourLogs+]� $��22��$��0��$��0��$��6��.+�o� $	��Top=�.@�m� $��_File��.@�m� $��	_New Tour�� m� Command �� *
AddNewTour+m� $��_New Tour Log�� �� *
addtourlog+m� $��_Open��m� $��_Save���� m� $��_Exit�� �� *Exit+m� $��_Edit��.@�m� $��_Delete Tour�� �� *DeleteWindow+m� $��_Modify Tour�� �� *ModifyWindow+m� $��_Options��.@�m� $��_Refresh�� �� *RefreshWindow+m� $��_Help��