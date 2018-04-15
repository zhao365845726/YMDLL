using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ML.ThirdParty.TencentCloud
{
    /// <summary>
    /// 点播帮助类
    /// </summary>
    public class DemandHelper : BaseHelper
    {
        #region 视频上传
        /// <summary>
        /// 发起视频上传   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string ApplyUpload(CreateClassParam ccp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["className"] = ccp.className;
            requestParams["parentId"] = ccp.parentId;
            return ReturnResult("ApplyUpload", requestParams);
        }

        /// <summary>
        /// 上传视频文件   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string UploadVodFile(CreateClassParam ccp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["className"] = ccp.className;
            requestParams["parentId"] = ccp.parentId;
            return ReturnResult("UploadVodFile", requestParams);
        }

        /// <summary>
        /// 确认视频上传   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string CommitUpload(CreateClassParam ccp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["className"] = ccp.className;
            requestParams["parentId"] = ccp.parentId;
            return ReturnResult("CommitUpload", requestParams);
        }

        /// <summary>
        /// URL 拉取视频上传   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string MultiPullVodFile(CreateClassParam ccp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["className"] = ccp.className;
            requestParams["parentId"] = ccp.parentId;
            return ReturnResult("MultiPullVodFile", requestParams);
        }
        #endregion

        #region 视频处理
        /// <summary>
        /// 使用任务流处理视频   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string RunProcedure(CreateClassParam ccp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["className"] = ccp.className;
            requestParams["parentId"] = ccp.parentId;
            return ReturnResult("RunProcedure", requestParams);
        }

        /// <summary>
        /// 对视频文件进行处理   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string ProcessFile(CreateClassParam ccp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["className"] = ccp.className;
            requestParams["parentId"] = ccp.parentId;
            return ReturnResult("ProcessFile", requestParams);
        }

        /// <summary>
        /// 视频转码   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string ConvertVodFile(CreateClassParam ccp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["className"] = ccp.className;
            requestParams["parentId"] = ccp.parentId;
            return ReturnResult("ConvertVodFile", requestParams);
        }

        /// <summary>
        /// 视频剪辑   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string ClipVideo(CreateClassParam ccp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["className"] = ccp.className;
            requestParams["parentId"] = ccp.parentId;
            return ReturnResult("ClipVideo", requestParams);
        }

        /// <summary>
        /// 视频拼接   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string ConcatVideo(CreateClassParam ccp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["className"] = ccp.className;
            requestParams["parentId"] = ccp.parentId;
            return ReturnResult("ConcatVideo", requestParams);
        }

        /// <summary>
        /// 指定时间点截图   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string CreateSnapshotByTimeOffset(CreateClassParam ccp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["className"] = ccp.className;
            requestParams["parentId"] = ccp.parentId;
            return ReturnResult("CreateSnapshotByTimeOffset", requestParams);
        }

        /// <summary>
        /// 截取雪碧图   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string CreateImageSprite(CreateClassParam ccp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["className"] = ccp.className;
            requestParams["parentId"] = ccp.parentId;
            return ReturnResult("CreateImageSprite", requestParams);
        }
        #endregion

        #region 媒资管理
        /// <summary>
        /// 获取视频信息   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string GetVideoInfo(GetVideoInfoParam gvip)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["fileId"] = gvip.fileld;
            if (!string.IsNullOrEmpty(gvip.infoFilter))
            {
                requestParams["infoFilter.n"] = gvip.infoFilter;
            }
            
            return ReturnResult("GetVideoInfo", requestParams);
        }

        /// <summary>
        /// 依照视频名称前缀获取视频信息   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string DescribeVodPlayInfo(DescribeVodPlayInfoParam dvpip)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["fileName"] = dvpip.fileName;
            requestParams["pageNo"] = dvpip.pageNo;
            requestParams["pageSize"] = dvpip.pageSize;
            return ReturnResult("DescribeVodPlayInfo", requestParams);
        }

        /// <summary>
        /// 依照 VID 查询视频信息   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string DescribeRecordPlayInfo(DescribeRecordPlayInfoParam drpip)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["vid"] = drpip.vid;
            return ReturnResult("DescribeRecordPlayInfo", requestParams);
        }

        /// <summary>
        /// 增加视频标签   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string CreateVodTags(CreateVodTagsParam cvtp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["fileId"] = cvtp.fileId;
            if(cvtp.tags.Length > 0)
            {
                for(int i = 0; i < cvtp.tags.Length; i++)
                {
                    requestParams["tags." + i.ToString()] = cvtp.tags[i];
                }
            }
            return ReturnResult("CreateVodTags", requestParams);
        }

        /// <summary>
        /// 删除视频标签   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string DeleteVodTags(DeleteVodTagsParam dvtp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["fileId"] = dvtp.fileId;
            if (dvtp.tags.Length > 0)
            {
                for (int i = 0; i < dvtp.tags.Length; i++)
                {
                    requestParams["tags." + i.ToString()] = dvtp.tags[i];
                }
            }
            return ReturnResult("DeleteVodTags", requestParams);
        }

        /// <summary>
        /// 删除视频   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string DeleteVodFile(DeleteVodFileParam dvfp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["fileId"] = dvfp.fileId;
            requestParams["isFlushCdn"] = dvfp.isFlushCdn;
            requestParams["priority"] = dvfp.priority;
            return ReturnResult("DeleteVodFile", requestParams);
        }

        /// <summary>
        /// 修改视频属性   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string ModifyVodInfo(ModifyVodInfoParam mvip)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["fileId"] = mvip.fileId;
            requestParams["fileName"] = mvip.fileName;
            requestParams["fileIntro"] = mvip.fileIntro;
            requestParams["classId"] = mvip.classId;
            requestParams["expireTime"] = mvip.expireTime;
            return ReturnResult("ModifyVodInfo", requestParams);
        }

        /// <summary>
        /// 增加打点信息   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string AddKeyFrameDesc(CreateClassParam ccp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["className"] = ccp.className;
            requestParams["parentId"] = ccp.parentId;
            return ReturnResult("AddKeyFrameDesc", requestParams);
        }

        /// <summary>
        /// 删除打点信息   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string DeleteKeyFrameDesc(CreateClassParam ccp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["className"] = ccp.className;
            requestParams["parentId"] = ccp.parentId;
            return ReturnResult("DeleteKeyFrameDesc", requestParams);
        }
        #endregion

        #region 视频分类管理
        /// <summary>
        /// 创建视频分类   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string CreateClass(CreateClassParam ccp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["className"] = ccp.className;
            if(ccp.parentId > 0)
            {
                requestParams["parentId"] = ccp.parentId;
            }
            return ReturnResult("CreateClass", requestParams);
        }

        /// <summary>
        /// 获取视频分类层次结构   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string DescribeAllClass()
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            return ReturnResult("DescribeAllClass", requestParams);
        }

        /// <summary>
        /// 获取视频分类信息   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string DescribeClass()
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            return ReturnResult("DescribeClass", requestParams);
        }

        /// <summary>
        /// 修改视频分类   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string ModifyClass(ModifyClassParam mcp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["classId"] = mcp.classId;
            requestParams["className"] = mcp.className;
            return ReturnResult("ModifyClass", requestParams);
        }

        /// <summary>
        /// 删除视频分类   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string DeleteClass(DeleteClassParam dcp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["classId"] = dcp.classId;
            return ReturnResult("DeleteClass", requestParams);
        }
        #endregion

        #region 事件通知与任务管理
        /// <summary>
        /// 拉取事件通知   最高调用频率：1000次/分钟
        /// </summary>
        /// <returns></returns>
        public string PullEvent()
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            return ReturnResult("PullEvent", requestParams);
        }

        /// <summary>
        /// 确认事件通知   最高调用频率：1000次/分钟
        /// </summary>
        /// <returns></returns>
        public string ConfirmEvent(ConfirmEventParam cep)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["msgHandle.n"] = cep.msgHandle_n;
            return ReturnResult("ConfirmEvent", requestParams);
        }

        /// <summary>
        /// 查询任务列表   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string GetTaskList(GetTaskListParam gtlp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["fileId"] = gtlp.fileld;
            requestParams["status"] = gtlp.status;
            requestParams["maxCount"] = gtlp.maxCount;
            requestParams["next"] = gtlp.next;
            return ReturnResult("GetTaskList", requestParams);
        }

        /// <summary>
        /// 查询任务详情   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string GetTaskInfo(GetTaskInfoParam gtip)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["vodTaskId"] = gtip.vodTaskId;
            return ReturnResult("GetTaskInfo", requestParams);
        }

        /// <summary>
        /// 重试任务   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string RedoTask(RedoTaskParam rtp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["vodTaskId"] = rtp.vodTaskId;
            return ReturnResult("RedoTask", requestParams);
        }
        #endregion

        #region 转码模版管理
        /// <summary>
        /// 创建转码模板   最高调用频率：100次/分钟
        /// 注意：用户创建的模板数最最多16个
        /// </summary>
        /// <returns></returns>
        public string CreateTranscodeTemplate(CreateTranscodeTemplateParam cttp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            requestParams["name"] = cttp.name;
            requestParams["container"] = cttp.container;
            requestParams["comment"] = cttp.comment;
            requestParams["isFiltrateVideo"] = cttp.isFiltrateVideo;
            requestParams["isFiltrateAudio"] = cttp.isFiltrateAudio;
            requestParams["video"] = cttp.video.video;
            requestParams["video.codec"] = cttp.video.codec;
            requestParams["video.fps"] = cttp.video.fps;
            requestParams["video.resolutionSelfAdapting"] = cttp.video.resolutionSelfAdapting;
            requestParams["video.width"] = cttp.video.width;
            requestParams["video.height"] = cttp.video.height;
            requestParams["video.bitrate"] = cttp.video.bitrate;
            requestParams["video.minGop"] = cttp.video.minGop;
            requestParams["video.maxGop"] = cttp.video.maxGop;
            requestParams["video.videoProfile"] = cttp.video.videoProfile;
            requestParams["video.colorSpace"] = cttp.video.colorSpace;
            requestParams["video.deinterlaced"] = cttp.video.deinterlaced;
            requestParams["video.videoRateControl"] = cttp.video.videoRateControl;
            requestParams["audio"] = cttp.audio.audio;
            requestParams["audio.codec"] = cttp.audio.codec;
            requestParams["audio.bitrate"] = cttp.audio.bitrate;
            requestParams["audio.soundSystem"] = cttp.audio.soundSystem;
            requestParams["audio.sampleRate"] = cttp.audio.sampleRate;
            return ReturnResult("CreateTranscodeTemplate", requestParams);
        }

        /// <summary>
        /// 查询转码模板列表   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string QueryTranscodeTemplateList(DeleteClassParam dcp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
      
            return ReturnResult("QueryTranscodeTemplateList", requestParams);
        }

        /// <summary>
        /// 查询转码模板   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string QueryTranscodeTemplate(DeleteClassParam dcp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);

            return ReturnResult("QueryTranscodeTemplate", requestParams);
        }

        /// <summary>
        /// 更新转码模板   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string UpdateTranscodeTemplate(DeleteClassParam dcp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);

            return ReturnResult("UpdateTranscodeTemplate", requestParams);
        }

        /// <summary>
        /// 删除转码模板   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string DeleteTranscodeTemplate(DeleteClassParam dcp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);

            return ReturnResult("DeleteTranscodeTemplate", requestParams);
        }
        #endregion

        #region 水印模板管理
        /// <summary>
        /// 申请上传水印文件   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string ApplyUploadWatermark(DeleteClassParam dcp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);

            return ReturnResult("ApplyUploadWatermark", requestParams);
        }

        /// <summary>
        /// 创建水印模板   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string CreateWatermarkTemplate(DeleteClassParam dcp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);

            return ReturnResult("CreateWatermarkTemplate", requestParams);
        }

        /// <summary>
        /// 查询水印模板列表   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string QueryWatermarkTemplateList(DeleteClassParam dcp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);

            return ReturnResult("QueryWatermarkTemplateList", requestParams);
        }

        /// <summary>
        /// 查询水印模板   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string QueryWatermarkTemplate(DeleteClassParam dcp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);

            return ReturnResult("QueryWatermarkTemplate", requestParams);
        }

        /// <summary>
        /// 更新水印模板   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string UpdateWatermarkTemplate(DeleteClassParam dcp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);

            return ReturnResult("UpdateWatermarkTemplate", requestParams);
        }

        /// <summary>
        /// 删除水印模板   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string DeleteWatermarkTemplate(DeleteClassParam dcp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);

            return ReturnResult("DeleteWatermarkTemplate", requestParams);
        }
        #endregion

        #region 密钥管理
        /// <summary>
        /// 获取视频解密密钥   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string DescribeDrmDataKey(DeleteClassParam dcp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            return ReturnResult("DescribeDrmDataKey", requestParams);
        }
        #endregion

        #region 数据统计
        /// <summary>
        /// 获取存储量   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string DescribeVodStorage(DeleteClassParam dcp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            return ReturnResult("DescribeVodStorage", requestParams);
        }

        /// <summary>
        /// 获取播放统计数据文件下载地址   最高调用频率：100次/分钟
        /// </summary>
        /// <returns></returns>
        public string GetPlayStatLogList(DeleteClassParam dcp)
        {
            SortedDictionary<string, object> requestParams = new SortedDictionary<string, object>(StringComparer.Ordinal);
            return ReturnResult("GetPlayStatLogList", requestParams);
        }
        #endregion
    }
}
