import 'package:flutter/material.dart';
import 'widgets/home_widget.dart';

void main() => runApp(App());

class App extends StatelessWidget {
 @override
 Widget build(BuildContext context) {
   return MaterialApp(
     title: 'Sensor Management System',
     debugShowCheckedModeBanner: false,
     home: Home(),
   );
 }
}