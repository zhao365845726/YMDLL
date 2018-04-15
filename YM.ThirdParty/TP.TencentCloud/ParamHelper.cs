using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ML.ThirdParty.TencentCloud
{
    #region 点播参数
    /// <summary>
    /// 点播公共请求参数
    /// </summary>
    public class DemandCommonRequestParam
    {
        /// <summary>
        /// [必填]接口指令的名称
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// [必填]区域参数，用来标识希望操作哪个区域的实例。
        /// 可选:         
        /// bj:北京        
        /// gz:广州
        /// sh:上海
        /// hk:香港
        /// ca:北美
        /// sg:新加坡
        /// usw:美西
        /// cd:成都
        /// de:德国
        /// kr:韩国 
        /// shjr:上海金融
        /// szjr:深圳金融
        /// gzopen:广州OPEN
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// [必填]当前UNIX时间戳
        /// </summary>
        public uint Timestamp { get; set; }

        /// <summary>
        /// [必填]随机正整数，与 Timestamp 联合起来, 用于防止重放攻击
        /// </summary>
        public uint Nonce { get; set; }

        /// <summary>
        /// [必填]由腾讯云平台上申请的标识身份的 SecretId 和 SecretKey, 其中 SecretKey 会用来生成 Signature
        /// </summary>
        public string SecretId { get; set; }

        /// <summary>
        /// [必填]请求签名，用来验证此次请求的合法性, 
        /// </summary>
        public string Signature { get; set; }
    }

    /// <summary>
    /// 获取视频信息
    /// </summary>
    public class GetVideoInfoParam
    {
        /// <summary>
        /// 希望获取的视频的ID
        /// </summary>
        public string fileld { get; set; }

        /// <summary>
        /// 指定需要返回的信息，可同时指定多个信息，n 从0开始递增。如果未填写该字段，默认返回所有信息.选项有：
        /// basicInfo（视频基础信息）
        /// metaData（视频元信息）
        /// drm（视频加密信息）
        /// transcodeInfo（视频转码结果信息）
        /// animatedGraphicsInfo（视频转动图结果信息）
        /// imageSpriteInfo（视频雪碧图信息）
        /// snapshotByTimeOffsetInfo（视频指定时间点截图信息）
        /// sampleSnapshotInfo（采样截图信息）
        /// keyFrameDescInfo（打点信息）
        /// </summary>
        public string infoFilter { get; set; }
    }

    /// <summary>
    /// 依照视频名称前缀获取视频信息
    /// </summary>
    public class DescribeVodPlayInfoParam
    {
        /// <summary>
        /// 视频名称(前缀匹配)
        /// </summary>
        public string fileName { get; set; }

        /// <summary>
        /// 页号
        /// </summary>
        public int pageNo { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int pageSize { get; set; }
    }

    /// <summary>
    /// 依照 VID 查询视频信息
    /// </summary>
    public class DescribeRecordPlayInfoParam
    {
        /// <summary>
        /// 直播/互动直播系统返回的video_id
        /// </summary>
        public string vid { get; set; }
    }

    /// <summary>
    /// 增加视频标签
    /// </summary>
    public class CreateVodTagsParam
    {
        /// <summary>
        /// 视频的ID
        /// </summary>
        public string fileId { get; set; }

        /// <summary>
        /// 标签组
        /// </summary>
        public string[] tags { get; set; }
    }

    /// <summary>
    /// 删除视频标签
    /// </summary>
    public class DeleteVodTagsParam
    {
        /// <summary>
        /// 视频的ID
        /// </summary>
        public string fileId { get; set; }

        /// <summary>
        /// 标签组
        /// </summary>
        public string[] tags { get; set; }
    }

    /// <summary>
    /// 删除视频
    /// </summary>
    public class DeleteVodFileParam
    {
        /// <summary>
        /// 文件id
        /// </summary>
        public string fileId { get; set; }

        /// <summary>
        /// 删除文件时是否及时刷新cdn缓存文件；默认不刷新，指定为1时刷新
        /// </summary>
        public int isFlushCdn { get; set; }

        /// <summary>
        /// 可填0；优先级0:中 1：高 2：低
        /// </summary>
        public int priority { get; set; }
    }

    /// <summary>
    /// 修改视频属性
    /// </summary>
    public class ModifyVodInfoParam
    {
        /// <summary>
        /// 文件id
        /// </summary>
        public string fileId { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string fileName { get; set; }

        /// <summary>
        /// 文件描述
        /// </summary>
        public string fileIntro { get; set; }

        /// <summary>
        /// 分类id
        /// </summary>
        public int classId { get; set; }

        /// <summary>
        /// 视频过期时间，格式: Y-m-d H:i:s，如2017-10-01 00:00:00。视频过期之后，该视频及其所有附属对象（转码结果、雪碧图等）将都被删除
        /// </summary>
        public string expireTime { get; set; }
    }

    /// <summary>
    /// 创建视频分类参数
    /// </summary>
    public class CreateClassParam
    {
        /// <summary>
        /// [必填]分类信息
        /// </summary>
        public string className { get; set; }

        /// <summary>
        /// [选填]父分类的id号，若不填，默认生成一级分类
        /// </summary>
        public int parentId { get; set; }
    }

    /// <summary>
    /// 修改视频分类
    /// </summary>
    public class ModifyClassParam
    {
        /// <summary>
        /// [必填]待修改的分类id
        /// </summary>
        public int classId { get; set; }

        /// <summary>
        /// [必填]新的分类名
        /// </summary>
        public string className { get; set; }
    }

    /// <summary>
    /// 删除视频分类
    /// </summary>
    public class DeleteClassParam
    {
        /// <summary>
        /// [必填]待删除视频分类的ID
        /// </summary>
        public int classId { get; set; }
    }

    /// <summary>
    /// 确认事件通知
    /// </summary>
    public class ConfirmEventParam
    {
        /// <summary>
        /// 事件句柄，n 从0开始递增。开发者获取到事件句柄后，等待确认的有效时间为30秒
        /// </summary>
        public string msgHandle_n { get; set; }
    }

    /// <summary>
    /// 查询任务列表参数
    /// </summary>
    public class GetTaskListParam
    {
        /// <summary>
        /// 文件ID，该字段若填写，则只会查询该文件的任务列表
        /// </summary>
        public string fileld { get; set; }

        /// <summary>
        /// 任务状态，有WAITING/PROCESSING/FINISH 三种
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 本次查询返回多少个数据，范围在10-100之间，不填默认为10
        /// </summary>
        public int maxCount { get; set; }

        /// <summary>
        /// 分批拉取时使用： 当列表数据比较多时，单次接口调用无法拉取整个列表，这时会返回拉取到的最后一个数据的ID，下次请求携带该ID，将会从该ID下一个开始拉取
        /// </summary>
        public string next { get; set; }
    }

    /// <summary>
    /// 查询任务详情参数
    /// </summary>
    public class GetTaskInfoParam
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public string vodTaskId { get; set; }
    }

    /// <summary>
    /// 重试任务参数
    /// </summary>
    public class RedoTaskParam
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public string vodTaskId { get; set; }
    }

    /// <summary>
    /// 创建转码模板
    /// </summary>
    public class CreateTranscodeTemplateParam
    {
        /// <summary>
        /// 转码模板的名字，长度必须小于64字节。 默认为空字符串
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 封装格式，支持mp4，flv，hls
        /// </summary>
        public string container { get; set; }

        /// <summary>
        /// 对该模板的描述，长度必须小于256字节。 默认为空字符串
        /// </summary>
        public string comment { get; set; }

        /// <summary>
        /// 去除视频数据，1为去除，0为保留，默认为0
        /// </summary>
        public int isFiltrateVideo { get; set; }

        /// <summary>
        /// 去除音频数据，1为去除，0为保留，默认为0
        /// </summary>
        public int isFiltrateAudio { get; set; }

        /// <summary>
        /// 视频对象参数
        /// </summary>
        public VideoParam video { get; set; }

        /// <summary>
        /// 音频对象参数
        /// </summary>
        public AudioParam audio { get; set; }
    }

    /// <summary>
    /// 视频对象参数
    /// </summary>
    public class VideoParam
    {
        /// <summary>
        /// 视频流配置信息，当isFiltrateVideo为1，则该字段将被忽略
        /// </summary>
        public object video { get; set; }

        /// <summary>
        /// 视频流的编码格式，可填 libx264（H264编码），libx265（H265编码），目前H265编码必须指定分辨率，并且需要在640*480以内
        /// </summary>
        public string codec { get; set; }

        /// <summary>
        /// 帧率，取值 24、25、30等，单位：hz
        /// </summary>
        public float fps { get; set; }

        /// <summary>
        /// 分辨率开启自适应：open为开启，close为关闭。 若为open，则width的值用于较长边，height的值用于较短边（长边的值不得短于短边）, 默认为open
        /// </summary>
        public string resolutionSelfAdapting { get; set; }

        /// <summary>
        /// 视频流宽度的最大值，若不填或者填0，而video.height填写了非0值，则按比例缩放，如果video.height也没填或者填0，则表示同源。 该值最小128，最大1920，单位：px
        /// </summary>
        public int width { get; set; }

        /// <summary>
        /// 视频流高度的最大值，若不填或者填0，而video.width填写了非0值，则按比例缩放，如果video.width也没填或者填0，则表示同源。 该值最小128，最大1920，单位：px
        /// </summary>
        public int height { get; set; }

        /// <summary>
        /// 视频流的码率，大于等于128，小于等于10000，单位：kbps
        /// </summary>
        public int bitrate { get; set; }

        /// <summary>
        /// 视频关键帧最小间隔，取值范围1~10
        /// </summary>
        public int minGop { get; set; }

        /// <summary>
        /// 视频关键帧最大间隔，取值范围1~10
        /// </summary>
        public int maxGop { get; set; }

        /// <summary>
        /// 视频编码档次，H.264编码档次可选择以上三种之一,默认为High，H.265则默认只能为Main
        /// </summary>
        public string videoProfile { get; set; }

        /// <summary>
        /// 视频色度空间，H.264只支持yuv420p，H.265支持yuv420p或者yuv420p10le
        /// </summary>
        public string colorSpace { get; set; }

        /// <summary>
        /// 视频去隔行模式，1：去隔行，0：保持视频隔行模式
        /// </summary>
        public int deinterlaced { get; set; }

        /// <summary>
        /// 视频压缩模式，0表示one pass，1表示two pass
        /// </summary>
        public int videoRateControl { get; set; }
    }

    /// <summary>
    /// 音频对象参数
    /// </summary>
    public class AudioParam
    {
        /// <summary>
        /// 音频流配置信息
        /// </summary>
        public object audio { get; set; }

        /// <summary>
        /// 音频流的编码格式，目前有libfdk_aac（更适合mp4, hls），libmp3lame（更适合flv）
        /// </summary>
        public string codec { get; set; }

        /// <summary>
        /// 音频流的码率，大于等于26，小于等于256，单位：kbps
        /// </summary>
        public int bitrate { get; set; }

        /// <summary>
        /// 音频通道方式，可填 1：双通道，2：双通道，默认为2
        /// </summary>
        public int soundSystem { get; set; }

        /// <summary>
        /// 音频流的采样率，可填 44100，48000，单位：hz
        /// </summary>
        public int sampleRate { get; set; }
    }
    #endregion

}
