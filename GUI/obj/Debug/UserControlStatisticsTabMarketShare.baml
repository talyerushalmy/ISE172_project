   M S B A M L   `   `   `  ����U  QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf  5      > :GUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:GUIGUI 5      -   'GUI.UserControlStatisticsTabMarketShare   5      <lvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf  5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      #localclr-namespace:GUI     5      .��5
      � .+�5   
      LiveCharts.Wpf.PieChart  -   5        $��PieChartMarketShare��5      &    LiveCharts.Wpf.Charts.Base.Chart    LegendLocation$  Bottom=�63      	Hoverable . $��0,37,-0.2,3q�6�      DataTooltip 6\   Z�    Series 6s   �� *MarketShareCollection+5      �� 5   
   -   5         ��Name$ comboBoxRange��5      $
��Left=�6*   $��	10,10,0,0q�6E   $	��Top=�6X   $	��120��6p   .@�5      ��  �� 
IsSelected . .��6-   	Last hour68   �� 5      .��6   
Last day6&   �� 5      .��6   	Last week6'   �� 5      .��6   Last ... trades6-   �� 5      .��6   
Between...6(   5      �� 5   
   -   5         ��Name$ buttonSearch��5      $��Search��6'   $
��Left=�68   $��
432,12,0,0q�6S   $	��Top=�6g   $��75��6   $y�Hidden=�6�   �� 5   
   -   5        $ comboBoxInputNumber��5      $
��Left=�60   $��
146,10,0,0q�6K   $	��Top=�6_   $��74��6w   $y�Hidden=�6�   .@�5      ��  . .��6.   500064   �� 5      .��6   100006#   �� 5      .��6   300006#   �� 5      .��6   1000006$   5      (  "System.Windows.Controls.DatePicker  5   
   -   5        $��DatePickerFrom��5      $
��Left=�6+   $��
159,10,0,0q�6F   $	��Top=�6Z   $y�Hidden=�6r     5   
   -   5        $��DatePickerTo��5      $
��Left=�6)   $��
275,10,0,0q�6D   $	��Top=�6X   $y�Hidden=�6p   5      5      