import 'package:flutter/material.dart';
import 'placeholder_widget.dart';
import 'package:sensor_management_system/widgets/sensorsList.dart';
import 'package:sensor_management_system/widgets/sensorPropertiesWidgets/sensorPropertiesList.dart';

class HomeRoute extends StatefulWidget {
  @override
  State<StatefulWidget> createState() {
    return _HomeRouteState();
  }
}

class _HomeRouteState extends State<HomeRoute> {
  int _currentIndex = 0;
  final List<Widget> _children = [
    PlaceholderWidget(Colors.red),
    PlaceholderWidget(Colors.orange),
    SensorPropertiesList()
  ];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar (
        title: Text('Sensor Management System')
      ),
      body: _children[_currentIndex],
      bottomNavigationBar: BottomNavigationBar(
        onTap: onTabTapped,
        currentIndex: _currentIndex,
        items: [
          BottomNavigationBarItem(
            icon: new Icon(Icons.shopping_cart),
            title: new Text('Store')
          ),
          BottomNavigationBarItem(
            icon: new Icon(Icons.settings_remote),
            title: new Text('Sensors')
          ),
          BottomNavigationBarItem(
            icon: new Icon(Icons.settings),
            title: new Text('Sensor Properties')
          )
        ],
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: () => {
          //print('insert here the logic for create')
        },
        child: const Icon(Icons.add),
        tooltip: 'Create'
      ),
      floatingActionButtonLocation: FloatingActionButtonLocation.endFloat,
    );
  }

  void onTabTapped(int index) {
    setState(() {
      _currentIndex = index;
    });
  }
}