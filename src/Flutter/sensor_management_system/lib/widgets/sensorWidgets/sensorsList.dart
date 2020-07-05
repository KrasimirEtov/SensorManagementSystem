import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';
import 'package:sensor_management_system/models/sensor.dart';
import 'package:sensor_management_system/models/sensorProperty.dart';
import 'package:sensor_management_system/services/webservice.dart';
import 'package:sensor_management_system/widgets/sensorWidgets/updateSensorRoute.dart';

class SensorsList extends StatefulWidget {
  @override
  State<StatefulWidget> createState() {
    return _SensorsListSate();
  }
}

class _SensorsListSate extends State<SensorsList> {
  List<Sensor> _sensors = List<Sensor>();
  List<SensorProperty> _sensorProperties = List<SensorProperty>();
  Future<String> _shouldDeleteSensorFuture;
  Future _deleteSensorFuture;

  @override
  void initState() {
    super.initState();
    _populateSensors();
  }

  void _populateSensors() {
    WebService().load(Sensor.all).then((sensors) => {
          setState(() => {_sensors = sensors})
        });

    WebService().load(SensorProperty.all).then((sensorProperties) => {
          setState(() => {_sensorProperties = sensorProperties})
        });
  }

  int _deleteSensor(int index) {
    int result = -1;
    int temp = 1;
    int count = 0;
    while (temp == 1) {
      _shouldDeleteSensorFuture.then((value) {
        result = int.parse(value);
        temp = 0;
        print(count++);
        if (result == 0) {
          WebService().delete(
              Sensor.initResourceByIdWithoutResponse(_sensors[index].id));
        }
      });
    }
    print('result before return is: ' + result.toString());
    return result;
  }

  void _shouldDeleteSensor(int index) {
    _shouldDeleteSensorFuture = WebService()
        .fetch(Sensor.initResourceByIdWithIntResponse(_sensors[index].id));
  }

  SensorProperty _getSensorProperty(int index) {
    var sensor = _sensors[index];
    var sensorProperty = _sensorProperties
        .firstWhere((element) => element.id == sensor.sensorPropertyId);
    return sensorProperty;
  }

  String _showSensorRangeIfPresent(int index) {
    String result = '';
    var sensor = _sensors[index];
    var sensorProperty = _sensorProperties
        .firstWhere((element) => element.id == sensor.sensorPropertyId);
    if (sensorProperty.isSwitch.toLowerCase() == 'false') {
      result = 'Min Range: ' +
          sensor.minRangeValue +
          '\n' +
          'Max Range: ' +
          sensor.maxRangeValue;
    }
    return result;
  }

  Card _buildItemsForListView(BuildContext context, int index) {
    return Card(
        child: ListTile(
      title: Text(
        'Type: ' +
            _getSensorProperty(index).measureType +
            '\n' +
            'Measure Unit: ' +
            _getSensorProperty(index).measureUnit +
            '\n' +
            'Description: ' +
            _sensors[index].description +
            '\n' +
            'Polling Interval: ' +
            _sensors[index].pollingInterval +
            '\n' +
            _showSensorRangeIfPresent(index),
      ),
      subtitle: Text('Created on: ' + _sensors[index].createdOn),
      trailing: PopupMenuButton(
          itemBuilder: (context) => [
                PopupMenuItem(
                    value: Text('Edit'),
                    child: ListTile(
                      leading: const Icon(Icons.edit),
                      title: Text('Edit'),
                      onTap: () {
                        Navigator.push(
                            context,
                            MaterialPageRoute(
                                builder: (context) => UpdateSensorRoute(
                                    id: _sensors[index].id))).then((value) {
                          setState(() {initState();});
                        });
                      },
                    )),
                PopupMenuItem(
                    value: Text('Remove'),
                    child: ListTile(
                      leading: const Icon(Icons.delete),
                      title: Text('Remove'),
                      onTap: () async {
                        int isUsedCount = int.parse(await WebService().fetch(
                            Sensor.initResourceByIdWithIntResponse(
                                _sensors[index].id)));
                        if (isUsedCount == 0) {
                          await WebService()
                              .delete(Sensor.initResourceByIdWithoutResponse(
                                  _sensors[index].id))
                              .whenComplete(() {
                            setState(() => {_sensors.removeAt(index)});
                            Scaffold.of(context).showSnackBar(SnackBar(
                                content:
                                    Text('Sensor was successfully deleted!')));
                          });
                        } else {
                          Scaffold.of(context).showSnackBar(SnackBar(
                              content: Text('Sensor is used by ' +
                                  isUsedCount.toString() +
                                  ' user sensors and can not be deleted!')));
                        }
                      },
                    ))
              ]),
    ));
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        body: ListView.builder(
            itemBuilder: _buildItemsForListView, itemCount: _sensors.length));
  }
}
