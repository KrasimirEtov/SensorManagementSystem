import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:sensor_management_system/models/sensorProperty.dart';
import 'package:sensor_management_system/models/sensor.dart';
import 'package:sensor_management_system/services/webservice.dart';

class UpdateSensorForm extends StatefulWidget {
  final String id;

  UpdateSensorForm({this.id});

  @override
  State<StatefulWidget> createState() {
    return _UpdateSensorFormState(this.id);
  }
}

class _UpdateSensorFormState extends State<UpdateSensorForm> {
  String id;
  final _formKey = GlobalKey<FormState>();
  Sensor _model = Sensor();
  List<Sensor> _sensors = List<Sensor>();
  List<SensorProperty> _sensorProperties = List<SensorProperty>();
  SensorProperty _selectedSensorPropertyFromDropdown;
  String _tempMinRangeValue = "0";
  String _tempMaxRangeValue = "0";
  String _tempPollingInterval = "0";
  Future _loadSensor;

  _UpdateSensorFormState(String id) {
    this.id = id;
  }

  @override
  void initState() {
    super.initState();
    _loadSensor = _populateSensor();
    _populateSensorProperties();
    _populateSensors();
  }

  Future _populateSensor() {
    return WebService()
        .load(Sensor.initResourceByIdWithResponse(this.id))
        .then((sensor) => {
              setState(() {
                _model = sensor;
                _tempMinRangeValue = _model.minRangeValue;
                _tempMaxRangeValue = _model.maxRangeValue;
                _tempPollingInterval = _model.pollingInterval;
              })
            });
  }

  Future _populateSensorProperties() {
    return WebService().load(SensorProperty.all).then((sensorProperties) => {
          setState(() {
            _sensorProperties = sensorProperties;
          })
        });
  }

  Future _populateSensors() {
    return WebService().load(Sensor.all).then((sensors) => {
          setState(() => {_sensors = sensors})
        });
  }

  Future _updateSensor() {
    return WebService().update(Sensor.initWithJsonBody(_model));
  }

  @override
  Widget build(BuildContext context) {
    return FutureBuilder(
      future: _loadSensor,
      builder: (context, snapshot) {
        if (snapshot.hasData) {
          _selectedSensorPropertyFromDropdown = _sensorProperties
              .firstWhere((element) => element.id == _model.sensorPropertyId);
          return Form(
              key: _formKey,
              child: SingleChildScrollView(
                  child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: <Widget>[
                  TextFormField(
                    initialValue: _model.description,
                    decoration: InputDecoration(
                        labelText: 'Description',
                        icon: Icon(Icons.description)),
                    validator: (value) {
                      if (value.isEmpty) {
                        return 'Description is required';
                      }
                      if (value.length < 3) {
                        return 'Description must have at least 3 characters!';
                      }
                      return null;
                    },
                    onChanged: (newValue) {
                      _model.description = newValue;
                    },
                  ),
                  TextFormField(
                    initialValue: _model.pollingInterval,
                    keyboardType: TextInputType.numberWithOptions(),
                    decoration: InputDecoration(
                        labelText: 'Polling Interval', icon: Icon(Icons.timer)),
                    validator: (value) {
                      if (value.isEmpty) {
                        return 'Polling Interval is required';
                      }
                      if (int.parse(value) < 0) {
                        return 'Polling Interval should be a positive value!';
                      }
                      if (_sensors.any((e) =>
                          double.parse(e.minRangeValue) ==
                              double.parse(_tempMinRangeValue) &&
                          double.parse(e.maxRangeValue) ==
                              double.parse(_tempMaxRangeValue) &&
                          int.parse(e.pollingInterval) ==
                              int.parse(_tempPollingInterval) &&
                          e.sensorPropertyId ==
                              _selectedSensorPropertyFromDropdown.id &&
                          e.id != this.id)) {
                        return 'Sensor with this Polling Interval, Min and Max Range already exists!';
                      }
                      return null;
                    },
                    onChanged: (newValue) {
                      setState(() {
                        _tempPollingInterval = newValue;
                      });
                      _model.pollingInterval = _tempPollingInterval;
                    },
                  ),
                  DropdownButton<SensorProperty>(
                      hint: Text('Select Sensor Type'),
                      value: _selectedSensorPropertyFromDropdown,
                      items: _sensorProperties.map((e) {
                        return new DropdownMenuItem<SensorProperty>(
                          value: e,
                          child: new Text(e.measureType),
                        );
                      }).toList(),
                      onChanged: (value) {
                        setState(() {
                          _selectedSensorPropertyFromDropdown = value;
                          if (_selectedSensorPropertyFromDropdown.isSwitch
                                  .toLowerCase() ==
                              'true') {
                            _model.minRangeValue = null;
                            _model.maxRangeValue = null;
                          }
                        });
                        _model.sensorPropertyId =
                            _selectedSensorPropertyFromDropdown.id;
                      }),
                  TextFormField(
                    initialValue: _model.minRangeValue,
                    readOnly: _selectedSensorPropertyFromDropdown.isSwitch
                            .toLowerCase() ==
                        'true',
                    keyboardType:
                        TextInputType.numberWithOptions(decimal: true),
                    decoration: InputDecoration(
                        labelText: 'Min Range Value',
                        icon: Icon(Icons.arrow_downward)),
                    validator: (value) {
                      if (_selectedSensorPropertyFromDropdown.isSwitch
                              .toLowerCase() ==
                          'true') {
                        print(_selectedSensorPropertyFromDropdown.isSwitch
                            .toLowerCase());
                        _model.minRangeValue = null;
                        return null;
                      }
                      if (value.isEmpty) {
                        return 'Min Range Value is required';
                      }
                      if (double.parse(value) >
                          double.parse(_tempMaxRangeValue)) {
                        return 'Min Range shold be less than Max Range!';
                      }
                      if (_sensors.any((e) =>
                          double.parse(e.minRangeValue) ==
                              double.parse(_tempMinRangeValue) &&
                          double.parse(e.maxRangeValue) ==
                              double.parse(_tempMaxRangeValue) &&
                          int.parse(e.pollingInterval) ==
                              int.parse(_tempPollingInterval) &&
                          e.sensorPropertyId ==
                              _selectedSensorPropertyFromDropdown.id &&
                          e.id != this.id)) {
                        return 'Sensor with this Polling Interval, Min and Max Range already exists!';
                      }

                      return null;
                    },
                    onChanged: (newValue) {
                      setState(() {
                        _tempMinRangeValue = newValue;
                      });
                      _model.minRangeValue = _tempMinRangeValue;
                    },
                  ),
                  TextFormField(
                    initialValue: _model.maxRangeValue,
                    readOnly: _selectedSensorPropertyFromDropdown.isSwitch
                            .toLowerCase() ==
                        'true',
                    keyboardType:
                        TextInputType.numberWithOptions(decimal: true),
                    decoration: InputDecoration(
                        labelText: 'Max Range Value',
                        icon: Icon(Icons.arrow_upward)),
                    validator: (value) {
                      if (_selectedSensorPropertyFromDropdown.isSwitch
                              .toLowerCase() ==
                          'true') {
                        _model.maxRangeValue = null;
                        return null;
                      }
                      if (value.isEmpty) {
                        return 'Max Range Value is required';
                      }
                      if (double.parse(value) <
                          double.parse(_tempMinRangeValue)) {
                        return 'Max Range should be more than Min Range!';
                      }
                      if (_sensors.any((e) =>
                          double.parse(e.minRangeValue) ==
                              double.parse(_tempMinRangeValue) &&
                          double.parse(e.maxRangeValue) ==
                              double.parse(_tempMaxRangeValue) &&
                          int.parse(e.pollingInterval) ==
                              int.parse(_tempPollingInterval) &&
                          e.sensorPropertyId ==
                              _selectedSensorPropertyFromDropdown.id &&
                          e.id != this.id)) {
                        return 'Sensor with this Polling Interval, Min and Max Range already exists!';
                      }
                      return null;
                    },
                    onChanged: (newValue) {
                      setState(() {
                        _tempMaxRangeValue = newValue;
                      });
                      _model.maxRangeValue = _tempMaxRangeValue;
                    },
                  ),
                  Padding(
                    padding: const EdgeInsets.symmetric(vertical: 16.0),
                    child: RaisedButton(
                      onPressed: () async {
                        // Validate returns true if the form is valid, or false
                        // otherwise.
                        if (_formKey.currentState.validate()) {
                          // If the form is valid, display a Snackbar.
                          await _updateSensor().then((value) {
                            Scaffold.of(context).showSnackBar(
                                SnackBar(content: Text('Updated entity!')));
                          }).catchError(() {
                            Scaffold.of(context).showSnackBar(SnackBar(
                                content: Text(
                                    'Error when trying to update entity!')));
                          });
                        }
                      },
                      child: Text('Update'),
                    ),
                  ),
                ],
              )));
        } else {
          return CircularProgressIndicator();
        }
      },
    );
  }
}
