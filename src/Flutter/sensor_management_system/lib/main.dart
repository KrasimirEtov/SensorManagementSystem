import 'package:flutter/material.dart';
import 'widgets/homeRoute.dart';

void main() {
  try {
    runApp(App());
  } on Exception catch (ex) {
    print('error: $ex');
  }
}

class App extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Sensor Management System',
      debugShowCheckedModeBanner: false,
      home: HomeRoute(),
    );
  }
}
