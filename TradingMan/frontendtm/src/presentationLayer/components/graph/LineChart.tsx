import { IgrFinancialChart } from 'igniteui-react-charts';
import { IgrFinancialChartModule } from 'igniteui-react-charts';
import StockData from "../../../dataLayer/models/StockData";

// Graph made based on tutorial at https://www.infragistics.com/products/ignite-ui-react/react/components/charts/types/stock-chart

IgrFinancialChartModule.register();

export interface ILineChartProps {
    data: StockData[],
    symbol: string,
}

/**
 * Element repsenting line chart with available stock data information.
 * @param props
 * @constructor
 */
function LineChart (props: ILineChartProps) {
    return (
        <div className="container" style={{height: "calc(100% - 25px)"}}>
                <IgrFinancialChart
                    width="100%"
                    height="100%"
                    chartType="Auto"
                    thickness={2}
                    chartTitle={props.symbol}
                    yAxisMode="Numeric"
                    yAxisTitle="Numeric Changed"
                    dataSource={props.data} />
            </div>
    );

}

export default LineChart;