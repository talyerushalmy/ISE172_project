   M S B A M L   `   `   `  ����U  QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf  5      > :GUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:GUIGUI 5      0   *GUI.UserControlStatisticsTabCommodityRates   5      <lvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf  5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      #localclr-namespace:GUI     5      .��5
      � .+�5   
   #   LiveCharts.Wpf.CartesianChart  $��28,64,22.8,4q�6I   &    LiveCharts.Wpf.Charts.Base.Chart    Series  6   �� *CommodityRatesCollection+   AxisY 5         LiveCharts.Wpf.Axis     Title$ Price��6   6,      AxisX 5        $
 Date��6      Labels 6(   �� *Labels+6D   5      �� 5   
   -   5         ��Name$ comboBoxRange��5      $
��Left=�6*   $��	10,10,0,0q�6E   $	��Top=�6X   $	��120��6p   .@�5      ��  �� 
IsSelected . .��65   	Last hour6@   �� 5      .��6$   
Last day6.   �� 5      .��6$   Last ... trades65   �� 5      .��6$   
Between...60   5      �� 5      -   5         ��Name$ buttonSearch��5      $��Search��63   $
��Left=�6D   $��
432,12,0,0q�6_   $	��Top=�6s   $��75��6�   $y�Hidden=�6�   �� 5      -   5        $ comboBoxInputNumber��5      $
��Left=�6<   $��
146,10,0,0q�6W   $	��Top=�6k   $��74��6�   $y�Hidden=�6�   .@�5      ��  . .��6:   50006@   �� 5      .��6(   100006/   �� 5      .��6(   300006/   �� 5      .��6(   10000060   5      (  "System.Windows.Controls.DatePicker  5       -   5        $��DatePickerFrom��5    !   $
��Left=�67   $��
159,10,0,0q�6R   $	��Top=�6f   $y�Hidden=�6~     5!      -   5        $��DatePickerTo��5!   !   $
��Left=�65   $��
275,10,0,0q�6P   $	��Top=�6d   $y�Hidden=�6|   �� 5"      -   5        $ comboBoxCommID��5"      $
��Left=�67   $��	90,37,0,0q�6R   $	��Top=�6e   $��40��6}   .@�5#      ��  . .��69   06<   �� 5$      .��6(   16+   �� 5%      .��6(   26+   �� 5&      .��6(   36+   �� 5'      .��6(   46+   �� 5(      .��6(   56+   �� 5)      .��6(   66+   �� 5*      .��6(   76+   �� 5+      .��6(   86+   �� 5,      .��6(   96+   5-      �� 5.      $��
Commodity:��6   $
��Left=�61   $��	10,37,0,0q�6L   $	��Top=�6_   5/      50      