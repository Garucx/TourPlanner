   M S B A M L   `   `   `  ����F  BTourPlanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:TourPlanner.ViewTourPlanner.View  5   	   $    TourPlanner.View.AddTourDialog   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:TourPlanner.View     5   	     � Title$  AddTourDialog��5   	   $	��600��6   $	��250��6+   $	��250��68   $	��600��6H   $	��250��6W   $	��600��6g   .��5	      � .+�5
   
   �� $��Start��6   $
��Left=�6    $��	10,10,0,0q�6;   $	��Top=�6N   $
��Bold"�6f   �� 5   
   $��Destination��6   $
��Left=�6&   $��
310,10,0,0q�6A   $	��Top=�6U   $
��Bold"�6m   �� 5   
   $��Address��6   $
��Left=�6"   $��	10,41,0,0q�6=   $	��Top=�6P   �� 5   
   $��	Area Code��6   $
��Left=�6$   $��	10,72,0,0q�6?   $	��Top=�6R   �� 5   
   $
��City��6   $
��Left=�6   $��
10,103,0,0q�6:   $	��Top=�6N   �� 5   
   $��Country��6   $
��Left=�6"   $��
10,134,0,0q�6=   $	��Top=�6Q   �� 5   
   $��Address��6   $
��Left=�6"   $��
310,41,0,0q�6=   $	��Top=�6Q   �� 5   
   $��	Area Code��6   $
��Left=�6$   $��
310,72,0,0q�6?   $	��Top=�6S   �� 5   
   $
��City��6   $
��Left=�6   $��310,103,0,0q�6:   $	��Top=�6O   �� 5   
   $��Country��6   $
��Left=�6"   $��310,134,0,0q�6=   $	��Top=�6R   �� 5   
   -   5         ��Name$ tb_start_address��5      $��
Jurekgasse��6,   $
��Left=�6>   $��	80,45,0,0q�6Y    �� TextWrapping$
 Wrap=�6l   $	��Top=�6�   $	��120��6�   �� 5   
   -   5        $ tb_start_areacode��5      $
��1150��6-   $
��Left=�69   $��	80,76,0,0q�6T   $
 Wrap=�6g   $	��Top=�6{   $	��120��6�   �� 5   
   -   5        $ tb_start_city��5      $��Vienna��6)   $
��Left=�67   $��
80,107,0,0q�6R   $
 Wrap=�6f   $	��Top=�6z   $	��120��6�   �� 5   
   -   5        $ tb_start_state��5      $��Austria��6+   $
��Left=�6:   $��
80,138,0,0q�6U   $
 Wrap=�6i   $	��Top=�6}   $	��120��6�   �� 5   
   -   5        $ tb_dest_address��5      $��Arnsteingasse��6+   $
��Left=�6@   $��
390,45,0,0q�6[   $
 Wrap=�6o   $	��Top=�6�   $	��120��6�   �� 5   
   -   5        $ tb_dest_areacode��5      $
��1150��6,   $
��Left=�68   $��
390,76,0,0q�6S   $
 Wrap=�6g   $	��Top=�6{   $	��120��6�   �� 5   
   -   5        $ tb_dest_city��5      $��Vienna��6(   $
��Left=�66   $��390,107,0,0q�6Q   $
 Wrap=�6f   $	��Top=�6z   $	��120��6�   �� 5   
   -   5        $ tb_dest_state��5      $��Austria��6)   $
��Left=�68   $��390,138,0,0q�6S   $
 Wrap=�6h   $	��Top=�6|   $	��120��6�   �� 5   
   -	   5         ��Name$ b_finish��5      $��Confirm��6#   $
��Left=�65   $��
80,182,0,0q�6P   $	��Top=�6d   $	��430��6|   6�   5      5      