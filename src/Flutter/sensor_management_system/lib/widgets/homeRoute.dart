import 'package:flutter/material.dart';
import 'placeholder_widget.dart';
import 'package:sensor_management_system/widgets/sensorPropertiesWidgets/sensorPropertiesList.dart';
import 'package:sensor_management_system/widgets/sensorWidgets/sensorsList.dart';
import 'package:sensor_management_system/widgets/sensorPropertiesWidgets/createSensorPropertyRoute.dart';
import 'package:sensor_management_system/widgets/sensorWidgets/createSensorRoute.dart';

class HomeRoute extends StatefulWidget {
  @override
  State<StatefulWidget> createState() {
    return _HomeRouteState();
  }
}

class _HomeRouteState extends State<HomeRoute> {
  int _currentIndex = 0;
  final List<Widget> _children = [
    SensorsList(),
    SensorPropertiesList()
  ];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text('Sensor Management System')),
      body: _children[_currentIndex],
      bottomNavigationBar: BottomNavigationBar(
        onTap: onTabTapped,
        currentIndex: _currentIndex,
        items: [
          BottomNavigationBarItem(
              icon: new Icon(Icons.settings_remote),
              title: new Text('Sensors')),
          BottomNavigationBarItem(
              icon: new Icon(Icons.settings),
              title: new Text('Sensor Properties'))
        ],
      ),
      floatingActionButton: FloatingActionButton(
          onPressed: () => {
                if (_currentIndex == 0)
                  {
                    Navigator.push(
                        context,
                        MaterialPageRoute(
                            builder: (context) => CreateSensorRoute()))
                  }
                else if (_currentIndex == 1)
                  {
                    Navigator.push(
                        context,
                        MaterialPageRoute(
                            builder: (context) => CreateSensorPropertyRoute()))
                  }
              },
          child: const Icon(Icons.add),
          tooltip: 'Create'),
      floatingActionButtonLocation: FloatingActionButtonLocation.centerDocked,
    );
  }

  void onTabTapped(int index) {
    setState(() {
      _currentIndex = index;
    });
  }
}
