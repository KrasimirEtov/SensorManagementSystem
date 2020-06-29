import 'package:conditional_builder/conditional_builder.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:sensor_management_system/models/sensorProperty.dart';
import 'package:sensor_management_system/models/sensor.dart';
import 'package:sensor_management_system/services/webservice.dart';

class CreateSensorForm extends StatefulWidget {
  @override
  State<StatefulWidget> createState() {
    return _CreateSensorFormState();
  }
}

class _CreateSensorFormState extends State<CreateSensorForm> {
  final _formKey = GlobalKey<FormState>();
  Sensor _model = Sensor();
  List<Sensor> _sensors = List<Sensor>();
  List<SensorProperty> _sensorProperties = List<SensorProperty>();
  bool _isSwitch = false;
  int _tempSensorPropertyId = 3;
  int _tempMinRangeValue = 0;
  int _tempMaxRangeValue = 0;

  @override
  void initState() {
    super.initState();
    _populateSensorProperties();
    _populateSensors();
  }

  void _createSensor() {
    WebService().send(Sensor.initWithJsonBody(_model)).whenComplete(() {});
  }

  Future _populateSensorProperties() {
    return WebService().load(SensorProperty.all).then((sensorProperties) => {
          setState(() => {_sensorProperties = sensorProperties})
        });
  }

  Future _populateSensors() {
    return WebService().load(Sensor.all).then((sensors) => {
          setState(() => {_sensors = sensors})
        });
  }

  bool _isSensorSwitch() {
    return _sensorProperties
            .firstWhere(
                (element) => element.id == _tempSensorPropertyId.toString())
            .isSwitch
            .toLowerCase() ==
        'true';
  }

  @override
  Widget build(BuildContext context) {
    return Form(
        key: _formKey,
        child: SingleChildScrollView(
            child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: <Widget>[
            TextFormField(
              decoration: InputDecoration(labelText: 'Descriptions'),
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
              keyboardType: TextInputType.number,
              inputFormatters: <TextInputFormatter>[
                WhitelistingTextInputFormatter.digitsOnly
              ],
              decoration: InputDecoration(
                  labelText: 'Polling Interval', icon: Icon(Icons.timer)),
              validator: (value) {
                if (value.isEmpty) {
                  return 'Polling Interval is required';
                }
                if (int.parse(value) < 0) {
                  return 'Polling Interval should be a positive value!';
                }
                return null;
              },
              onChanged: (newValue) {
                _model.pollingInterval = newValue;
              },
            ),
            ConditionalBuilder(
              condition: _isSensorSwitch(),
              builder: (context) {
                return TextFormField(
                  keyboardType: TextInputType.number,
                  inputFormatters: <TextInputFormatter>[
                    WhitelistingTextInputFormatter.digitsOnly
                  ],
                  decoration: InputDecoration(
                      labelText: 'Min Range Value',
                      icon: Icon(Icons.arrow_drop_down_circle)),
                  validator: (value) {
                    if (value.isEmpty) {
                      return 'Min Range Value is required';
                    }
                    if (int.parse(value) > _tempMaxRangeValue) {
                      return 'Min Range shold be less than Max Range!';
                    }
                    return null;
                  },
                  onChanged: (newValue) {
                    _model.minRangeValue = newValue;
                    setState(() {
                      _tempMinRangeValue = int.parse(newValue);
                    });
                  },
                );
              },
            ),
            Padding(
              padding: const EdgeInsets.symmetric(vertical: 16.0),
              child: RaisedButton(
                onPressed: () {
                  // Validate returns true if the form is valid, or false
                  // otherwise.
                  if (_formKey.currentState.validate()) {
                    // If the form is valid, display a Snackbar.
                    Scaffold.of(context).showSnackBar(
                        SnackBar(content: Text('Processing Data')));
                    _createSensor();
                  }
                },
                child: Text('Create'),
              ),
            ),
          ],
        )));
  }
}
